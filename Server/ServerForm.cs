using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Windows.Forms;

using ChatTCP.Common;

namespace ChatTCP.Server
{
    public partial class ServerForm : Form
    {
        private enum Stato
        {
            Iniziale,
            Listening
        }
        private Stato _stato;

        private const int DIMBUFF = 64;

        private readonly byte[] receivedBytesBuffer = new byte[DIMBUFF];
        private string receivedString = "";

        private TcpListener _listener;
        private readonly List<TcpClient> _clients = new List<TcpClient>();
        private readonly Dictionary<TcpClient, string> _clientToUsername = new Dictionary<TcpClient, string>();

        public ServerForm()
        {
            InitializeComponent();
        }

        private void ServerForm_Load(object sender, EventArgs e)
        {
            // Imposta porta di default
            PortaTcpTextBox.Text = Convert.ToString(Protocol.DEFAULT_PORT);

            // Imposta lo stato iniziale
            _stato = Stato.Iniziale;
            UpdateLayout();

            string ipV4 = "";

			var host = Dns.GetHostEntry(Dns.GetHostName());
			foreach (var ip in host.AddressList)
			{
				if (ip.AddressFamily == AddressFamily.InterNetwork)
				{
                    ipV4 = ip.ToString();
				}
			}

			this.Text = $"ChatTCP Server - {ipV4}";
        }

        private void ServerForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            StopListener();
        }

        private void StartListeningButton_Click(object sender, EventArgs e)
        {
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

            // Crea socket di ricezione
            _listener = new TcpListener(IPAddress.Any, port);

            try
            {
                // Avvio listening sulla porta TCP nota
                _listener.Start();

                // Aggiorna lo stato e la UI
                _stato = Stato.Listening;
                UpdateLayout();

                Log("Server avviato");

                // Creazione funzione di callback per accettare connessioni
                _listener.BeginAcceptTcpClient(new AsyncCallback(OnAccept), null);
            }
            catch (Exception ex) when (ex is SocketException || ex is IOException)
            {
                Log($"StartListening: Errore: {ex.Message}");
                MessageBox.Show(ex.Message);
                StopListener();
                return;
            }
        }

        private void StopListeningButton_Click(object sender, EventArgs e)
        {
            StopListener();

            Log("Server fermato");
        }

        private void DisconnectEveryoneButton_Click(object sender, EventArgs e)
        {
            DisconnectAllClients();

            Log("Disconnessi tutti i client");
        }

        /// <summary>
        /// Disconnette un client e lo rimuove dalla lista di clients conosciuti
        /// </summary>
        /// <param name="client">il client da disconnettere</param>
        private void DisconnectClient(TcpClient client)
        {
            if (client != null)
            {
                // Rimuovi l'oggetto dalla lista prima di distruggerlo
                _clientToUsername.Remove(client);
                _clients.Remove(client);

                // Prova a distruggere l'oggetto
                try
                {
                    // Chiudi la stream
                    var stream = client.GetStream();
                    try
                    {
                        stream?.Close();
                    }
                    catch (ObjectDisposedException)
                    {
                        // Ci abbiamo provato
                    }

                    // Chiudi definitivamente il client
                    client?.Close();
                }
                catch (ObjectDisposedException)
                {
                    // Ci abbiamo provato
                }
            }

            OnOnlineUsersUpdated();
        }

        /// <summary>
        /// Disconnetti tutti i client connessi
        /// </summary>
        private void DisconnectAllClients()
        {
            // Crea una copia della lista perchè durante l'iterazione non
            // si può modificare la lista originale
            foreach (TcpClient client in _clients.ToList())
            {
                DisconnectClient(client);
            }

            // Svuota la lista di client
            _clientToUsername.Clear();
            _clients.Clear();
        }

        /// <summary>
        /// Disconnetti tutti i client connessi e ferma il listener
        /// </summary>
        private void StopListener()
        {
            // Disconnetti tutti i client
            DisconnectAllClients();

            // Ferma il listener
            try
            {
                _listener?.Stop();
            }
            catch (Exception ex) when (ex is SocketException || ex is IOException)
            {
                Log($"StopListener: Errore: {ex.Message}");
            }
            _listener = null;

            // Aggiorna lo stato e la UI
            _stato = Stato.Iniziale;
            UpdateLayout();
        }

        /// <summary>
        /// Chiama questa funzione quando la lista degli utenti online è cambiata
        /// Questa funzione si occupa di informare i client di ciò
        /// </summary>
        private void OnOnlineUsersUpdated()
        {
            HashSet<string> onlineUsers = new HashSet<string>();

            foreach (var clients in _clientToUsername)
            {
                onlineUsers.Add(clients.Value);
            }

            var updatedOnlineUsersMessage = new Protocol.UpdatedOnlineUsersMessage
            {
                online_users = onlineUsers.ToList(),
            };
            byte[] messageBytes = Protocol.EncodeMessage(updatedOnlineUsersMessage.ToJson());

            foreach (var clients in _clientToUsername.Keys)
            {
                clients.GetStream().Write(messageBytes, 0, messageBytes.Length);
            }
        }

        private delegate void del_OnAccept(IAsyncResult asyn);
        public void OnAccept(IAsyncResult asyn)
        {
            // Fa in modo che questa funzione venga eseguita nel thread corretto
            if (InvokeRequired)
            {
                BeginInvoke(new del_OnAccept(OnAccept), asyn);
                return;
            }

            if (_listener == null)
            {
                return;
            }

            try
            {
                var client = _listener.EndAcceptTcpClient(asyn);

                _clients.Add(client);

                var stream = client.GetStream();

                try
                {
                    stream.BeginRead(receivedBytesBuffer, 0, receivedBytesBuffer.Length, new AsyncCallback(OnDataReceived), client);
                }
                catch (Exception ex) when (ex is SocketException || ex is IOException)
                {
                    Log($"BeginRead(): Errore: {ex.Message}");
                    DisconnectClient(client);
                }

                // Torna ad ascoltare nuove richieste di connessione
                _listener.BeginAcceptTcpClient(new AsyncCallback(OnAccept), null);
            }
            catch (Exception ex) when (ex is SocketException || ex is IOException)
            {
                Log($"OnAccept(): Errore: {ex.Message}");
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

            var client = (TcpClient)asyn.AsyncState;

            if (client == null)
            {
                return;
            }

            if (!client.Connected)
            {
                DisconnectClient(client);
                return;
            }

            var stream = client.GetStream();

            try
            {
                int numReceivedBytes = stream.EndRead(asyn);

                if (numReceivedBytes == 0)
                {
                    DisconnectClient(client);
                    return;
                }

                // Processa il messaggio ricevuto
                string szData = Protocol.DecodeMessage(receivedBytesBuffer, numReceivedBytes);
                DatiRxTextBox.Text += szData;

                receivedString += szData;

                var messages = Protocol.GetMessages(ref receivedString);
                foreach (var messageText in messages)
                {
                    Protocol.BaseMessage message = Protocol.FromJson(messageText);

                    if (message is Protocol.LoginMessage loginMessage)
                    {
                        // Fai il login
                        ConnectionDB connection = new ConnectionDB();
                        var loginResultMessage = connection.Login(loginMessage.username, loginMessage.password);

                        if (loginResultMessage.result == Protocol.LoginResultMessage.Result.Success)
                        {
                            _clientToUsername[client] = loginMessage.username;

                            // Aggiorna gli altri client del nuovo utente connesso
                            OnOnlineUsersUpdated();
                        }

                        // Invia il risultato
                        var bytes = Protocol.EncodeMessage(loginResultMessage.ToJson());
                        stream.Write(bytes, 0, bytes.Length);
                    }
                    else if (message is Protocol.RegisterMessage registerMessage)
                    {
                        // Fai la registrazione
                        ConnectionDB connection = new ConnectionDB();
                        var loginResultMessage = connection.Register(registerMessage.nome, registerMessage.cognome, registerMessage.email, registerMessage.username, registerMessage.password);

                        if (loginResultMessage.result == Protocol.LoginResultMessage.Result.Success)
                        {
                            _clientToUsername[client] = registerMessage.username;

                            // Aggiorna gli altri client del nuovo utente connesso
                            OnOnlineUsersUpdated();
                        }

                        // Invia il risultato
                        var bytes = Protocol.EncodeMessage(loginResultMessage.ToJson());
                        stream.Write(bytes, 0, bytes.Length);
                    }
                    else if (message is Protocol.SendMessageMessage sendMessageMessage)
                    {
                        // Invia il messaggio agli altri client
                        _clientToUsername.TryGetValue(client, out string username);

                        if (username != null)
                        {
                            var messageReceivedMessage = new Protocol.MessageReceivedMessage
                            {
                                timestamp = Protocol.DateTimeOffsetToUNIXTimestamp(Protocol.DateTimeOffsetNow),
                                username = username,
                                to_users = sendMessageMessage.to_users,
                                message = sendMessageMessage.message
                            };
                            byte[] bytes = Protocol.EncodeMessage(messageReceivedMessage.ToJson());

                            // Determiniamo i client che devono ricevere il messaggio
                            HashSet<TcpClient> targetClients = new HashSet<TcpClient>();
                            if (sendMessageMessage.to_users != null && sendMessageMessage.to_users.Count > 0)
                            {
                                targetClients = (from kvp in _clientToUsername
                                                 where sendMessageMessage.to_users.Contains(kvp.Value)
                                                 select kvp.Key).ToHashSet();
                            }
                            else
                            {
                                targetClients = _clientToUsername.Keys.ToHashSet();
                            }

                            foreach (var targetClient in targetClients)
                            {
                                if (!targetClient.Connected)
                                {
                                    Log("Client non connesso");
                                    DisconnectClient(targetClient);
                                    continue;
                                }

                                if (targetClient != client)
                                {
                                    NetworkStream streamClient = targetClient.GetStream();
                                    streamClient.Write(bytes, 0, bytes.Length);
                                }
                            }

                            AddMessageToUI(Protocol.UNIXTimestampToDateTimeOffset(messageReceivedMessage.timestamp), messageReceivedMessage.username, messageReceivedMessage.message);
                        }
                    }
                    else if (message is Protocol.UpdateUserInfoMessage updateUserInfoMessage)
                    {
                        // Aggiorna nome e cognome
                        if (_clientToUsername.ContainsKey(client))
                        {
                            var username = _clientToUsername[client];
                            ConnectionDB connection = new ConnectionDB();
                            connection.UpdateUserInfo(username, updateUserInfoMessage.nome, updateUserInfoMessage.cognome, updateUserInfoMessage.email);
                        }
                        else
                        {
                            Log("Un utente non loggato ha provato ad aggiornare i dati dell'account");
                        }
                    }
                    else if (message is Protocol.ChangePasswordMessage changePasswordMessage)
                    {
                        // Aggiorna nome e cognome
                        if (_clientToUsername.ContainsKey(client))
                        {
                            var username = _clientToUsername[client];
                            ConnectionDB connection = new ConnectionDB();
                            connection.ChangePassword(username, changePasswordMessage.new_password);
                        }
                        else
                        {
                            Log("Un utente non loggato ha provato a cambiare la password");
                        }
                    }
                    else if (message is Protocol.LogoutMessage logoutMessage)
                    {
                        // Rimuove l'associazione riferita all'utente che vuole svolgere il Logout dalla sessione
                        if (_clientToUsername.ContainsKey(client))
                        {
                            _clientToUsername.Remove(client);

                            // Aggiorna gli altri client dell'utente disconnesso
                            OnOnlineUsersUpdated();
                        }
                        else
                        {
                            Log("Un utente non loggato ha provato a sloggarsi");
                        }
                    }
                    else
                    {
                        Log("Messaggio sconosciuto ricevuto dal client");
                        Log(messageText);
                    }
                }

                // Torna ad ascoltare nuovi messaggi
                stream?.BeginRead(receivedBytesBuffer, 0, receivedBytesBuffer.Length, new AsyncCallback(OnDataReceived), client);
            }
            catch (Exception ex) when (ex is SocketException || ex is IOException)
            {
                Log($"OnDataReceived(): SocketException: {ex.Message}");
                DisconnectClient(client);
            }
        }

        private void UpdateLayout()
        {
            switch (_stato)
            {
                case Stato.Iniziale:
                    SettingsGroupBox.Enabled = true;
                    DatiRxGroupBox.Enabled = false;
                    StartListeningButton.Enabled = true;
                    StopListeningButton.Enabled = false;
                    DisconnectEveryoneButton.Enabled = false;
                    break;
                case Stato.Listening:
                    SettingsGroupBox.Enabled = false;
                    DatiRxGroupBox.Enabled = true;
                    StartListeningButton.Enabled = false;
                    StopListeningButton.Enabled = true;
                    DisconnectEveryoneButton.Enabled = true;
                    break;
            }
        }

        private void AddMessageToUI(DateTimeOffset date, string username, string message)
        {
            Log($"{date:HH:mm:ss} {username}: {message}");
        }

        private int intLogCount = 0;
        private void Log(string message)
        {
            LogListBox.Items.Add(intLogCount.ToString().PadLeft(3) + " " + message);
            LogListBox.SelectedIndex = LogListBox.Items.Count - 1;
            intLogCount++;
        }
    }
}
