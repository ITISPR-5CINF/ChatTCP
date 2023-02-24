using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Windows.Forms;

using ChatTCP.Common;

namespace ChatTCP.Client
{
    public partial class ClientForm : Form
    {
        private enum Stato
        {
            Disconnesso,
            Connessione,
            Connesso,
            Loggato,
        }
        private Stato _stato;

        private const int DIMBUFF = 64;

        private readonly byte[] receivedBytesBuffer = new byte[DIMBUFF];
        private string receivedString = "";

        private TcpClient _clientSocket;
        private NetworkStream _stream;

        private string _username;

        public ClientForm()
        {
            InitializeComponent();
        }

        private void ClientForm_Load(object sender, EventArgs e)
        {
            // Imposta porta di default
            PortaTcpTextBox.Text = Convert.ToString(Protocol.DEFAULT_PORT);

            // Ricavo tutti i nomi dei pc in rete
            List<string> NetComputers = BroadcastDomainUtils.GetNetComputers();
            // Aggiungo il computer locale
            NetComputers.Insert(0, "localhost");
            // Li inserisco nel combo
            for (int i = 0; i < NetComputers.Count; i++)
            {
                NetworkComputersComboBox.Items.Add(NetComputers[i]);
            }
            // Seleziono localhost di default
            NetworkComputersComboBox.SelectedIndex = 0;
            NetworkComputersComboBox.Text = (string)NetworkComputersComboBox.Items[NetworkComputersComboBox.SelectedIndex];

            // Aggiungo la callback per gestire il tasto enter nella TextBox
            SendTextBox.KeyUp += SendTextBox_KeyUp;

            // Imposta lo stato iniziale
            _stato = Stato.Disconnesso;
            AggiornaLayout();
        }

        private void ClientForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            CloseConnection();
        }

        private void ConnectButton_Click(object sender, EventArgs e)
        {
            string computerName = (string)NetworkComputersComboBox.Items[NetworkComputersComboBox.SelectedIndex];

            string ipAddressString = HostnameToIpAddress(computerName);
            if (ipAddressString == "")
            {
                MessageBox.Show($"Non è stato possibile ottenere l'indirizzo IP di {computerName}");
                return;
            }

            // Converti da hostname a indirizzo IP se necessario
            IsLocalIpV4Address(ipAddressString, ref ipAddressString);

            if (!ControllaIndirizzoIpV4(ipAddressString))
            {
                MessageBox.Show($"L'indirizzo IP {ipAddressString} non è valido");
                return;
            }

            string portString = PortaTcpTextBox.Text;
            if (!int.TryParse(portString, out int port))
            {
                MessageBox.Show($"La porta {portString} non è valido");
                return;
            }

            if (port <= 1024)
            {
                MessageBox.Show($"La porta {port} non deve essere inferiore o uguale a 1024 (non usare porte standard)");
                return;
            }

            // Converti l'indirizzo IP in un intero a 32 bit
            IPAddress ipAddress = IPAddress.Parse(ipAddressString);
            long rawIpAddress = (uint)BitConverter.ToInt32(ipAddress.GetAddressBytes(), 0);

            // Aggiorna lo stato e la UI
            _stato = Stato.Connessione;
            AggiornaLayout();

            // Creazione del socket
            _clientSocket = new TcpClient();

            try
            {
                // Inizia la connessione con il server
                _clientSocket.BeginConnect(ipAddress, port, new AsyncCallback(OnConnect), null);
            }
            catch (Exception ex) when (ex is SocketException || ex is IOException)
            {
                Log($"Connect: Errore: {ex.Message}");
                MessageBox.Show(ex.Message);
                CloseConnection();
            }
        }

        private void CloseButton_Click(object sender, EventArgs e)
        {
            CloseConnection();
        }

        private void UserInfoButton_Click(object sender, EventArgs e)
        {
            var form = new FormUserInfo(_username, "", "", "")
            {
                OnUpdateUserInfoCallback = OnUpdateUserInfo,
                OnChangePasswordCallback = OnChangePassword
            };
            form.ShowDialog();
        }

        private void LogoutButton_Click(object sender, EventArgs e)
        {
            // Richiedi al server di fare logout
            var logoutMessage = new Protocol.LogoutMessage();

            var bytes = Protocol.EncodeMessage(logoutMessage.ToJson());

            try
            {
                _stream.Write(bytes, 0, bytes.Length);
            }
            catch (Exception ex) when (ex is SocketException || ex is IOException)
            {
                Log($"Logout: Errore: {ex.Message}");
                MessageBox.Show(ex.Message);
                CloseConnection();
                return;
            }

            // Rimuovi le informazioni del client connesso per liberare lo spazio in memoria per un altro
            _username = null;

            // Aggiorna lo stato e la UI
            _stato = Stato.Connesso;
            AggiornaLayout();

            // Riapri LoginForm
            if (!OpenLoginForm())
            {
                CloseConnection();
            }
        }

        private void SendTextBox_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                SendMessage();
                e.Handled = true;
            }
        }

        private void SendButton_Click(object sender, EventArgs e)
        {
            SendMessage();
        }

        private delegate void del_OnConnect(IAsyncResult asyn);
        public void OnConnect(IAsyncResult asyn)
        {
            // Fa in modo che questa funzione venga eseguita nel thread corretto
            if (InvokeRequired)
            {
                BeginInvoke(new del_OnConnect(OnConnect), asyn);
                return;
            }

            // Controlla che il client non sia nullo
            if (_clientSocket == null)
            {
                Log("OnConnect: Client nullo");
                MessageBox.Show("Errore: Client nullo");
                CloseConnection();
                return;
            }

            // Controlla che siamo ancora connessi con il server
            if (!_clientSocket.Connected)
            {
                Log("OnConnect: Impossibile connettersi");
                MessageBox.Show("Impossibile connettersi");
                CloseConnection();
                return;
            }

            try
            {
                _clientSocket.EndConnect(asyn);

                _stream = _clientSocket.GetStream();

                // Aggiorna lo stato e la UI
                _stato = Stato.Connesso;
                AggiornaLayout();

                // Apri il form
                if (!OpenLoginForm())
                {
                    // Chiudi la connessione
                    CloseConnection();
                }

                // Torna a ricevere nuovi dati
                _stream?.BeginRead(receivedBytesBuffer, 0, receivedBytesBuffer.Length, new AsyncCallback(OnDataReceived), _stream);
            }
            catch (Exception ex) when (ex is SocketException || ex is IOException)
            {
                Log($"OnConnect: Errore: {ex.Message}");
                MessageBox.Show(ex.Message);
                CloseConnection();
            }
        }

        private delegate void del_OnDataReceived(IAsyncResult asyn);
        public void OnDataReceived(IAsyncResult asyn)
        {
            // Fa in modo che questa funzione venga eseguita nel thread corretto
            if (InvokeRequired)
            {
                BeginInvoke(new del_OnDataReceived(OnDataReceived), asyn);
                return;
            }

            // Controlla che il client non sia nullo
            if (_clientSocket == null)
            {
                Log("OnDataReceived: Client nullo");
                CloseConnection();
                return;
            }

            // Controlla che siamo ancora connessi con il server
            if (!_clientSocket.Connected)
            {
                Log("OnDataReceived: Disconnesso dal client");
                CloseConnection();
                return;
            }

            try
            {
                int numReceivedBytes = _stream.EndRead(asyn);

                if (numReceivedBytes == 0)
                {
                    Log("OnDataReceived: Disconnesso dal client");
                    MessageBox.Show("Disconnesso dal server");
                    CloseConnection();
                    return;
                }

                // Processa il messaggio ricevuto
                string szData = Protocol.DecodeMessage(receivedBytesBuffer, numReceivedBytes);

                receivedString += szData;

                var messages = Protocol.GetMessages(ref receivedString);
                foreach (var messageText in messages)
                {
                    Protocol.BaseMessage message = Protocol.FromJson(messageText);

                    if (message is Protocol.LoginResultMessage loginResultMessage)
                    {
                        // Controlla se siamo loggati
                        if (loginResultMessage.result != Protocol.LoginResultMessage.Result.Success)
                        {
                            _username = null;

                            // Comunica all'utente il risultato
                            switch (loginResultMessage.result)
                            {
                                case Protocol.LoginResultMessage.Result.WrongCredentials:
                                    MessageBox.Show("Login fallito: Credenziali errate");
                                    break;
                                case Protocol.LoginResultMessage.Result.UserAlreadyExists:
                                    MessageBox.Show("Registrazione fallita: Username già esistente");
                                    break;
                                default:
                                    MessageBox.Show("Login/registrazione fallito: Errore sconosciuto");
                                    break;
                            }

                            // Riapri il form
                            if (!OpenLoginForm())
                            {
                                // Chiudi la connessione
                                CloseConnection();
                            }
                        }
                        else
                        {
                            // Aggiorna lo stato e la UI
                            _stato = Stato.Loggato;
                            AggiornaLayout();
                        }
                    }
                    else if (message is Protocol.MessageReceivedMessage messageReceivedMessage)
                    {
                        // Metti il messaggio nella UI
                        AddMessageToUI(Protocol.UNIXTimestampToDateTimeOffset(messageReceivedMessage.timestamp), messageReceivedMessage.username, messageReceivedMessage.message, messageReceivedMessage.to_users);
                    }
                    else if (message is Protocol.UpdatedOnlineUsersMessage updatedOnlineUsersMessage)
                    {
                        AddUserToOU(updatedOnlineUsersMessage.online_users);
                    }
                    else
                    {
                        Log("OnDataReceived: Messaggio sconosciuto ricevuto dal server");
                        Log(messageText);
                    }
                }

                // Torna ad ascoltare nuovi messaggi
                _stream?.BeginRead(receivedBytesBuffer, 0, receivedBytesBuffer.Length, new AsyncCallback(OnDataReceived), _stream);
            }
            catch (Exception ex) when (ex is SocketException || ex is IOException)
            {
                Log($"OnDataReceived: Errore: {ex.Message}");
                MessageBox.Show(ex.Message);
                CloseConnection();
                return;
            }
        }

        /// <summary>
        /// Apre il form per il login
        /// </summary>
        /// <returns>false se il login è stato annullato, true altrimenti</returns>
        private bool OpenLoginForm()
        {
            // Apri il form
            var formLogin = new FormLogin();
            formLogin.ShowDialog();

            if (formLogin.Cancelled)
            {
                return false;
            }

            if (!formLogin.isRegisteredInstruction)
            {
                var username = formLogin.Username;
                var password = formLogin.Password;

                _username = username;

                var loginMessage = new Protocol.LoginMessage
                {
                    username = username,
                    password = password
                };

                var messageBytes = Protocol.EncodeMessage(loginMessage.ToJson());

                try
                {
                    _stream.Write(messageBytes, 0, messageBytes.Length);
                }
                catch (Exception ex) when (ex is SocketException || ex is IOException)
                {
                    Log($"OpenLoginForm: Errore: {ex.Message}");
                    MessageBox.Show(ex.Message);
                    CloseConnection();
                    return false;
                }
            }
            else
            {
                var username = formLogin.UsernameRegister;
                var password = formLogin.PasswordRegister;
                var nome = formLogin.Nome;
                var cognome = formLogin.Cognome;
                var email = formLogin.Email;

                _username = username;

                var registerMessage = new Protocol.RegisterMessage
                {
                    username = username,
                    password = password,
                    nome = nome,
                    cognome = cognome,
                    email = email
                };

                var messageBytes = Protocol.EncodeMessage(registerMessage.ToJson());

                try
                {
                    _stream.Write(messageBytes, 0, messageBytes.Length);
                }
                catch (Exception ex) when (ex is SocketException || ex is IOException)
                {
                    Log($"OpenLoginForm: Errore: {ex.Message}");
                    MessageBox.Show(ex.Message);
                    CloseConnection();
                    return false;
                }
            }

            return true;
        }

        private void CloseConnection()
        {
            Log("CloseConnection: Disconnessione");

            // Chiudi la stream
            _stream?.Close();
            _stream = null;

            // Chiudi la connessione
            _clientSocket?.Close();
            _clientSocket = null;

            // Pulisci lo username
            _username = null;

            // Aggiorna lo stato e la UI
            _stato = Stato.Disconnesso;
            AggiornaLayout();
        }

        /// <summary>
        /// Invia il messaggio scritto nella TextBox
        /// </summary>
        private void SendMessage()
        {
            string messageText = SendTextBox.Text;

            // Non inviare un messaggio se vuoto
            if (string.IsNullOrEmpty(messageText))
            {
                return;
            }

            if (_clientSocket == null)
            {
                Log("Send: Client nullo");
                MessageBox.Show("Non connesso a nessun server");
                CloseConnection();
                return;
            }

            if (!_clientSocket.Connected)
            {
                Log("Send: Client non connesso");
                MessageBox.Show("Non connesso a nessun server");
                CloseConnection();
                return;
            }

            // Serializza il messaggio
            var toUsers = OnlineUsersCheckedListBox.CheckedItems.Cast<string>().ToHashSet().ToList();
            Protocol.SendMessageMessage sendMessageMessage = new Protocol.SendMessageMessage
            {
                message = messageText,
                to_users = toUsers
            };
            var messageBytes = Protocol.EncodeMessage(sendMessageMessage.ToJson());

            try
            {
                _stream.Write(messageBytes, 0, messageBytes.Length);
            }
            catch (Exception ex) when (ex is SocketException || ex is IOException)
            {
                Log($"Send: Errore: {ex.Message}");
                MessageBox.Show(ex.Message);
                CloseConnection();
                return;
            }

            // Aggiungi il messaggio nella UI
            AddMessageToUI(Protocol.DateTimeOffsetNow, _username, sendMessageMessage.message, toUsers);

            // Ripulisi la textbox
            SendTextBox.Text = "";
        }

        private void OnUpdateUserInfo(string nome, string cognome, string email)
        {
            var updateUserInfoMessage = new Protocol.UpdateUserInfoMessage
            {
                nome = nome,
                cognome = cognome,
                email = email
            };
            var messageBytes = Protocol.EncodeMessage(updateUserInfoMessage.ToJson());

            try
            {
                _stream.Write(messageBytes, 0, messageBytes.Length);
            }
            catch (Exception ex) when (ex is SocketException || ex is IOException)
            {
                Log($"OnUpdateUserInfo: Errore: {ex.Message}");
                MessageBox.Show(ex.Message);
                CloseConnection();
                return;
            }
        }

        private void OnChangePassword(string password)
        {
            var changePasswordMessage = new Protocol.ChangePasswordMessage
            {
                new_password = password
            };
            var messageBytes = Protocol.EncodeMessage(changePasswordMessage.ToJson());

            try
            {
                _stream.Write(messageBytes, 0, messageBytes.Length);
            }
            catch (Exception ex) when (ex is SocketException || ex is IOException)
            {
                Log($"OnChangePassword: Errore: {ex.Message}");
                MessageBox.Show(ex.Message);
                CloseConnection();
                return;
            }
        }

        private void AggiornaLayout()
        {
            switch (_stato)
            {
                case Stato.Disconnesso:
                    ImpostazioniGroupBox.Enabled = true;
                    AccountGroupBox.Enabled = false;
                    SendGroupBox.Enabled = false;
                    ConnectButton.Enabled = true;
                    CloseButton.Enabled = false;
                    OnlineUsersGroupBox.Enabled = false;
                    break;
                case Stato.Connessione:
                    ImpostazioniGroupBox.Enabled = false;
                    AccountGroupBox.Enabled = false;
                    SendGroupBox.Enabled = false;
                    ConnectButton.Enabled = false;
                    CloseButton.Enabled = false;
                    OnlineUsersGroupBox.Enabled = false;
                    break;
                case Stato.Connesso:
                    ImpostazioniGroupBox.Enabled = false;
                    AccountGroupBox.Enabled = false;
                    SendGroupBox.Enabled = false;
                    ConnectButton.Enabled = false;
                    CloseButton.Enabled = true;
                    OnlineUsersGroupBox.Enabled = false;
                    break;
                case Stato.Loggato:
                    ImpostazioniGroupBox.Enabled = false;
                    AccountGroupBox.Enabled = true;
                    SendGroupBox.Enabled = true;
                    ConnectButton.Enabled = false;
                    CloseButton.Enabled = true;
                    OnlineUsersGroupBox.Enabled = true;
                    break;
            }

            if (_stato != Stato.Loggato)
            {
                // Ripulisci la lista di utenti connessi
                OnlineUsersCheckedListBox.Items.Clear();
            }

            // Aggiorna IpRemotoLabel
            if (_stato != Stato.Disconnesso)
            {
                var ip = NetworkComputersComboBox.Text.Equals("localhost") ? "127.0.0.1" : NetworkComputersComboBox.Text;
                var port = PortaTcpTextBox.Text;

                IpRemotoLabel.Text = $"IP del server: {ip}:{port}";
            }
            else
            {
                IpRemotoLabel.Text = "IP remoto: ";
            }

            // Aggiorna l'username
            if (_stato == Stato.Loggato && _username != null)
            {
                ConnectedAsLabel.Text = $"Connesso come: {_username}";
            }
            else
            {
                ConnectedAsLabel.Text = "Non loggato";
            }
        }

        private bool IsLocalIpV4Address(string host, ref string ipaddress)
        {
            try
            { // get host IP addresses
                int i;
                int j;

                // Tutti gli indirizzi locali relativi all'indirizzo dato
                IPAddress[] hostIPs = Dns.GetHostAddresses(host);
                // Tutti gli indirizzi locali relativi a questa interfaccia
                IPAddress[] localIPs = Dns.GetHostAddresses(Dns.GetHostName());

                // Per ogni indirizzo locale possibile relativo all'indirizzo dato... 
                for (i = 0; i < hostIPs.Length; i++)
                {
                    // E' IpV4?
                    if (hostIPs[i].AddressFamily == AddressFamily.InterNetwork)
                    {
                        // L'indirizzo dato è locale?
                        if (IPAddress.IsLoopback(hostIPs[i]))
                        {
                            ipaddress = hostIPs[i].ToString();
                            return true;
                        }

                    }
                    // Per questo indirizzo locale, verifico se questa interfaccia ne ha uno uguale
                    for (j = 0; j < localIPs.Length; j++)
                    {
                        // E' IpV4?
                        if (localIPs[j].AddressFamily == AddressFamily.InterNetwork)
                        {
                            if (hostIPs[i].Equals(localIPs[j]))
                            {
                                ipaddress = localIPs[j].ToString();
                                return true;
                            }
                        }
                    }
                }
            }
            catch { }
            return false;
        }

        private bool ControllaIndirizzoIpV4(string strHost)
        {
            if (strHost.Contains(":"))
            {
                string[] hostParts = strHost.Split(':');

                if (hostParts.Length == 2)
                {
                    return int.TryParse(hostParts[1], out _);
                }
            }
            else
            {
                if (strHost.Contains("."))
                {
                    string[] hostDottedParts = strHost.Split('.');
                    if (hostDottedParts.Length == 4)
                    {
                        int j = 0;
                        for (int i = 0; i < hostDottedParts.Length; i++)
                        {
                            if (byte.TryParse(hostDottedParts[i], out _))
                            {
                                j++;
                            }
                        }
                        if (j == hostDottedParts.Length)
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        private string HostnameToIpAddress(string hostname)
        {
            string strIpDotted = "";

            IPAddress[] ipAddresses = Dns.GetHostAddresses(hostname);
            if (ipAddresses.Length > 0)
            {
                for (int i = 0; i < ipAddresses.Length; i++)
                {
                    if (ipAddresses[i].AddressFamily == AddressFamily.InterNetwork)
                    {
                        strIpDotted = ipAddresses[i].ToString();
                        break;
                    }
                }
            }

            return strIpDotted;
        }

        private void AddMessageToUI(DateTimeOffset date, string username, string message, List<string> toUsers = null)
        {
            if (toUsers != null && toUsers.Count > 0)
            {
                MessagesListBox.Items.Add($"{date:HH:mm:ss} - {username} -> {string.Join(", ", toUsers.ToArray())}: {message}");
            }
            else
            {
                MessagesListBox.Items.Add($"{date:HH:mm:ss} - {username}: {message}");
            }

            MessagesListBox.SelectedIndex = MessagesListBox.Items.Count - 1;
        }

        private void AddUserToOU(List<string> users)
        {
            OnlineUsersCheckedListBox.Items.Clear();
            foreach (string user in users)
            {
                // Non aggiungere l'utente con cui siamo loggati
                if (user == _username)
                {
                    continue;
                }

                OnlineUsersCheckedListBox.Items.Add(user);
            }
        }

        private int intLogCount = 0;
        private void Log(string message)
        {
            LoggingListBox.Items.Add(intLogCount.ToString().PadLeft(3) + " " + message);
            LoggingListBox.SelectedIndex = LoggingListBox.Items.Count - 1;
            intLogCount++;
        }
    }
}
