using System;
using System.Collections.Generic;
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
            Connesso
        }
        private Stato _stato;

        private const int DIMBUFF = 5;

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

            try
            {
                // Creazione del socket
                _clientSocket = new TcpClient();
                Log("CALL: Socket creato (" + ipAddressString + ":" + portString + ")");

                // Associazione dell'endpoint (indirizzo IP locale/porta TCP) al socket
                _clientSocket.BeginConnect(ipAddress, port, new AsyncCallback(OnConnect), null);
                Log("CALL: BeginConnect(); Richiesta connessione");

                // Aggiorna lo stato e la UI
                _stato = Stato.Connessione;
                AggiornaLayout();
            }
            catch (SocketException se)
            {
                MessageBox.Show(se.Message, "Client");
            }
        }

        private void CloseButton_Click(object sender, EventArgs e)
        {
            CloseConnection();
        }

        private void UserInfoButton_Click(object sender, EventArgs e)
        {
            var form = new FormUserInfo(_username, "", "")
            {
                OnUpdateUserInfoCallback = OnUpdateUserInfo,
                OnChangePasswordCallback = OnChangePassword
            };
            form.ShowDialog();
        }

        private void SendButton_Click(object sender, EventArgs e)
        {
            string messageText = SendTextBox.Text;

            // Non inviare un messaggio se vuoto
            if (string.IsNullOrEmpty(messageText))
            {
                return;
            }

            try
            {
                // Serializza il messaggio
                Protocol.SendMessageMessage sendMessageMessage = new Protocol.SendMessageMessage
                {
                    message = messageText
                };

                if (_clientSocket == null)
                {
                    Log("Client nullo");
                    return;
                }

                if (!_clientSocket.Connected)
                {
                    Log("Client non connesso");
                    return;
                }

                Log("CALL: Send();");
                var message = Protocol.EncodeMessage(sendMessageMessage.ToJson());
                _stream.Write(message, 0, message.Length);

                AddMessageToUI(_username, sendMessageMessage.message);
            }
            catch (SocketException se)
            {
                MessageBox.Show(se.Message, "Client");
            }

            // Ripulisi la textbox
            SendTextBox.Text = "";
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

            // Controlla che la funzione asincrona sia terminata
            if (!asyn.IsCompleted)
            {
                return;
            }

            // Controlla che la connessione sia avvenuta con successo
            if (!_clientSocket.Connected)
            {
                Log("EVNT: OnConnect(); Connessione NON accettata");
                MessageBox.Show("Impossibile connettersi", "Client");

                // Aggiorna lo stato e la UI
                _stato = Stato.Disconnesso;
                AggiornaLayout();

                return;
            }

            _clientSocket.EndConnect(asyn);
            Log("EVNT: OnConnect(); Connessione accettata");
            Log("CALL: BeginReceive(); Pronto a ricevere");
            _stream = _clientSocket.GetStream();

            // Apri il form
            if (!OpenLoginForm())
            {
                // Chiudi la connessione
                CloseConnection();
            }

            // Torna a ricevere nuovi dati
            _stream.BeginRead(receivedBytesBuffer, 0, receivedBytesBuffer.Length, new AsyncCallback(OnDataReceived), _stream);

            // Aggiorna lo stato e la UI
            _stato = Stato.Connesso;
            AggiornaLayout();
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

            try
            {
                if (_clientSocket == null || !_clientSocket.Connected)
                {
                    Log("EVNT: OnDataReceived(); Disconnesso dal Client");
                    return;
                }

                int numReceivedBytes = _stream.EndRead(asyn);

                if (numReceivedBytes == 0)
                {
                    Log("EVNT: OnDataReceived(); Disconnesso dal Server");
                    CloseConnection();
                    return;
                }

                Log("EVNT: OnDataReceived();");

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

                            // Riapri il form
                            if (!OpenLoginForm())
                            {
                                // Chiudi la connessione
                                CloseConnection();
                            }
                        }
                        else
                        {
                            UpdateUsername();
                        }
                    }
                    else if (message is Protocol.MessageReceivedMessage messageReceivedMessage)
                    {
                        // Metti il messaggio nella UI
                        AddMessageToUI(messageReceivedMessage.username, messageReceivedMessage.message);
                    }
                    else
                    {
                        Log("Messaggio sconosciuto ricevuto dal server");
                        Log(messageText);
                    }
                }

                // Torna ad ascoltare nuovi messaggi
                Log("CALL: BeginReceive(); Pronto a ricevere");
                _stream?.BeginRead(receivedBytesBuffer, 0, receivedBytesBuffer.Length, new AsyncCallback(OnDataReceived), _stream);
            }
            catch (SocketException se)
            {
                Log("EVNT: OnDataReceived(); Errore " + se.Message);
                MessageBox.Show(se.Message, "Client");
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
                _stream.Write(messageBytes, 0, messageBytes.Length);
            }
            else
            {
                var username = formLogin.UsernameRegister;
                var password = formLogin.PasswordRegister;
                var nome = formLogin.Nome;
                var cognome = formLogin.Cognome;

                _username = username;

                var registerMessage = new Protocol.RegisterMessage
                {
                    username = username,
                    password = password,
                    nome = nome,
                    cognome = cognome
                };

                var messageBytes = Protocol.EncodeMessage(registerMessage.ToJson());
                _stream.Write(messageBytes, 0, messageBytes.Length);
            }

            return true;
        }

        private void CloseConnection()
        {
            Log("CALL: CloseConnection(); Richiesta disconnessione");

            // Chiudi la stream
            _stream?.Close();
            _stream = null;

            // Chiudi la connessione
            _clientSocket?.Close();
            _clientSocket = null;

            // Pulisci lo username
            _username = null;
            UpdateUsername();

            // Aggiorna lo stato e la UI
            _stato = Stato.Disconnesso;
            AggiornaLayout();
        }

        private void UpdateUsername()
        {
            var usernameText = _username;
            if (usernameText == null)
            {
                usernameText = "[Disconnesso]";
            }

            ConnectedAsLabel.Text = $"Connesso come: {usernameText}";
        }

        private void OnUpdateUserInfo(string nome, string cognome)
        {
            var updateUserInfoMessage = new Protocol.UpdateUserInfoMessage
            {
                nome = nome,
                cognome = cognome
            };
            var bytes = Protocol.EncodeMessage(updateUserInfoMessage.ToJson());
            _stream.Write(bytes, 0, bytes.Length);
        }

        private void OnChangePassword(string password)
        {
            var changePasswordMessage = new Protocol.ChangePasswordMessage
            {
                new_password = password
            };
            var bytes = Protocol.EncodeMessage(changePasswordMessage.ToJson());
            _stream.Write(bytes, 0, bytes.Length);
        }

        private void AggiornaLayout()
        {
            switch (_stato)
            {
                case Stato.Disconnesso:
                    ImpostazioniGroupBox.Enabled = true;
                    SendGroupBox.Enabled = false;
                    ConnectButton.Enabled = true;
                    CloseButton.Enabled = false;
                    SendButton.Enabled = false;
                    UserInfoButton.Enabled = false;
                    break;
                case Stato.Connessione:
                    ImpostazioniGroupBox.Enabled = false;
                    SendGroupBox.Enabled = false;
                    ConnectButton.Enabled = false;
                    CloseButton.Enabled = false;
                    SendButton.Enabled = false;
                    UserInfoButton.Enabled = false;
                    break;
                case Stato.Connesso:
                    ImpostazioniGroupBox.Enabled = false;
                    SendGroupBox.Enabled = true;
                    ConnectButton.Enabled = false;
                    CloseButton.Enabled = true;
                    SendButton.Enabled = true;
                    UserInfoButton.Enabled = true;
                    break;
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

        private void AddMessageToUI(string username, string message)
        {
            MessagesListBox.Items.Add($"{username}: {message}");
            MessagesListBox.SelectedIndex = MessagesListBox.Items.Count - 1;
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
