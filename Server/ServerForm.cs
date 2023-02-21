using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Windows.Forms;

using ChatTCP.Common;

namespace ChatTCP.Server
{
    public partial class ServerForm : Form
    {
        private const string WINDOWTITLE = "Socket Server in C#";

        private enum Stato
        {
            Iniziale,
            Listening
        }
        private Stato _stato;

        private const int DEFAULT_PORT = 8221;

        private const int DIMBUFF = 5;

        private readonly byte[] receivedBytesBuffer = new byte[DIMBUFF];
        private string receivedString = null;

        private TcpListener _listener;
        private readonly List<TcpClient> _clients = new List<TcpClient>();
        private readonly Dictionary<TcpClient, String> _clientToUsername = new Dictionary<TcpClient, string>();

        public ServerForm()
        {
            InitializeComponent();
        }

        private void ServerForm_Load(object sender, EventArgs e)
        {
            // Imposta il titolo della finestra
            Text = WINDOWTITLE;

            // Imposta porta di default
            PortaTcpTextBox.Text = Convert.ToString(DEFAULT_PORT);

            // Imposta lo stato iniziale
            _stato = Stato.Iniziale;
            UpdateLayout();
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

            try
            {
                // Creazione del socket in attesa sulla porta nota
                Log("-------------------------");

                // Crea socket di ricezione
                _listener = new TcpListener(IPAddress.Any, port);
                Log($"CALL: Listener creato ({IPAddress.Any}:{portString})");

                // Avvio listening sulla porta TCP nota
                _listener.Start();
                Log("CALL: Listen(); Socket in listening");

                // Creazione funzione di callback per accettare connessioni
                _listener.BeginAcceptTcpClient(new AsyncCallback(OnAccept), null);
                Log("CALL: BeginAccept(); Callback BeginAccept impostata");

                // Aggiorna lo stato e la UI
                _stato = Stato.Listening;
                UpdateLayout();
            }
            catch (SocketException se)
            {
                MessageBox.Show(se.Message, "Server");
            }
        }

        private void StopListeningButton_Click(object sender, EventArgs e)
        {
            StopListener();
        }

        private void SendButton_Click(object sender, EventArgs e)
        {
            string messageText = DatiTxTextBox.Text;
            Protocol.MessageReceivedMessage messageReceivedMessage = new Protocol.MessageReceivedMessage
            {
                username = "admin",
                message = messageText
            };
            var messageBytes = Protocol.EncodeMessage(messageReceivedMessage.ToJson());

            foreach (var client in _clients.ToList())
            {
                if (!client.Connected)
                {
                    Log("Client non connesso");
                    DisconnectClient(client);
                    continue;
                }

                client.GetStream().Write(messageBytes, 0, messageBytes.Length);
            }

            Log($"{messageReceivedMessage.username}: {messageReceivedMessage.message}");
        }

        private void DisconnectButton_Click(object sender, EventArgs e)
        {
            DisconnectAllClients();
        }

        /// <summary>
        /// Disconnette un client e lo rimuove dalla lista di clients conosciuti
        /// </summary>
        /// <param name="client">il client da disconnettere</param>
        private void DisconnectClient(TcpClient client)
        {
            if (client == null)
            {
                return;
            }

            // Rimuovi l'oggetto dalla lista prima di distruggerlo
            _clientToUsername.Remove(client);
            _clients.Remove(client);

            // Prova a distruggere l'oggetto
            try {
                // Chiudi la stream
                var stream = client.GetStream();
                try
                {
                    stream?.Close();
                }
                catch (ObjectDisposedException) {
                    // Ci abbiamo provato
                }

                // Chiudi definitivamente il client
                client?.Close();
            } catch (ObjectDisposedException) {
                // Ci abbiamo provato
            }
        }

        /// <summary>
        /// Disconnetti tutti i client connessi
        /// </summary>
        private void DisconnectAllClients()
        {
            Log("CALL: DisconnectClients(); Richiesta disconnessione");

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
            Log("CALL: StopListener(); socket listener");

            // Disconnetti tutti i client
            DisconnectAllClients();

            // Ferma il listener
            _listener?.Stop();
            _listener = null;

            // Aggiorna lo stato e la UI
            _stato = Stato.Iniziale;
            UpdateLayout();
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
                Log("EVNT: BeginAccept(); Listener nullo");
                return;
            }

            try
            {
                var client = _listener.EndAcceptTcpClient(asyn);
                Log("EVNT: BeginAccept(); Connessione accettata");

                _clients.Add(client);

                Log("CALL: BeginReceive(); Pronto a ricevere");
                var stream = client.GetStream();
                stream.BeginRead(receivedBytesBuffer, 0, receivedBytesBuffer.Length, new AsyncCallback(OnDataReceived), client);

                // Torna ad ascoltare nuove richieste di connessione
                Log(".........................");
                Log("CALL: BeginAccept(); BeginAccept REimpostata");
                _listener.BeginAcceptTcpClient(new AsyncCallback(OnAccept), null);
            }
            catch (SocketException se)
            {
                Log("EVNT: BeginAccept(); Errore: " + se.Message);
                MessageBox.Show(se.Message, "Server");
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
                Log("EVNT: OnDataReceived(); Client nullo");
                return;
            }

            if (!client.Connected)
            {
                Log("EVNT: OnDataReceived(); Client non connesso");
                DisconnectClient(client);
                return;
            }

            try
            {
                var stream = client.GetStream();

                int numReceivedBytes = stream.EndRead(asyn);

                if (numReceivedBytes == 0)
                {
                    Log("EVNT: OnDataReceived(); Disconnesso dal Client");
                    DisconnectClient(client);
                    return;
                }

                Log("EVNT: OnDataReceived();");

                // Processa il messaggio ricevuto
                string szData = Protocol.DecodeMessage(receivedBytesBuffer, numReceivedBytes);
                DatiRxTextBox.Text += szData;

                if (receivedString == null)
                {
                    receivedString = szData;
                }
                else
                {
                    receivedString += szData;
                }
                string maybeJsonString = Protocol.GetMessageOrNull(receivedString);
                if (maybeJsonString != null)
                {
                    // Resetta il buffer della stringa ricevuta
                    receivedString = null;

                    Protocol.BaseMessage message = Protocol.FromJson(maybeJsonString);

                    if (message is Protocol.LoginMessage loginMessage)
                    {
                        // Fai il login
                        ConnectionDB connection = new ConnectionDB();
                        var loginResultMessage = connection.Login(loginMessage.username, loginMessage.password);

                        if (loginResultMessage.result == Protocol.LoginResultMessage.Result.Success)
                        {
                            _clientToUsername[client] = loginMessage.username;
                        }

                        // Invia il risultato
                        var bytes = Protocol.EncodeMessage(loginResultMessage.ToJson());
                        stream.Write(bytes, 0, bytes.Length);
                    }
                    else if (message is Protocol.RegisterMessage registerMessage)
                    {
                        // Fai la registrazione
                        ConnectionDB connection = new ConnectionDB();
                        var loginResultMessage = connection.Register(registerMessage.nome, registerMessage.cognome, registerMessage.username, registerMessage.password);

                        if (loginResultMessage.result == Protocol.LoginResultMessage.Result.Success)
                        {
                            _clientToUsername[client] = registerMessage.username;
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
                                username = username,
                                message = sendMessageMessage.message
                            };
                            byte[] bytes = Protocol.EncodeMessage(messageReceivedMessage.ToJson());

                            foreach (var clients in _clients.ToList())
                            {
                                if (!clients.Connected)
                                {
                                    Log("Client non connesso");
                                    DisconnectClient(clients);
                                    continue;
                                }

                                if (clients != client)
                                {
                                    NetworkStream streamClient = clients.GetStream();
                                    streamClient.Write(bytes, 0, bytes.Length);
                                }
                            }

                            Log($"{messageReceivedMessage.username}: {messageReceivedMessage.message}");
                        }
                    }
                    else
                    {
                        Log("Messaggio sconosciuto ricevuto dal client");
                        Log(maybeJsonString);
                    }
                }

                // Torna ad ascoltare nuovi messaggi
                Log("CALL: BeginReceive(); Pronto a ricevere");
                stream.BeginRead(receivedBytesBuffer, 0, receivedBytesBuffer.Length, new AsyncCallback(OnDataReceived), client);
            }
            catch (SocketException se)
            {
                Log("EVNT: OnDataReceived(); Errore " + se.Message);
                MessageBox.Show(se.Message, "Server");
            }
        }

        private void UpdateLayout()
        {
            switch (_stato)
            {
                case Stato.Iniziale:
                    SettingsGroupBox.Enabled = true;
                    DatiTxGroupBox.Enabled = false;
                    DatiRxGroupBox.Enabled = false;
                    StartListeningButton.Enabled = true;
                    StopListeningButton.Enabled = false;
                    SendButton.Enabled = false;
                    DisconnectButton.Enabled = false;
                    break;
                case Stato.Listening:
                    SettingsGroupBox.Enabled = false;
                    DatiTxGroupBox.Enabled = true;
                    DatiRxGroupBox.Enabled = true;
                    StartListeningButton.Enabled = false;
                    StopListeningButton.Enabled = true;
                    SendButton.Enabled = true;
                    DisconnectButton.Enabled = true;
                    break;
            }
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
