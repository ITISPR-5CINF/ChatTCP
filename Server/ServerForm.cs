using System;
using System.Net;
using System.Net.Sockets;
using System.Windows.Forms;
using System.Collections.Generic;

using ChatTCP.Common;

namespace ChatTCP.Server
{
    public partial class ServerForm : Form
    {
        private const string WINDOWTITLE = "Socket Server in C#";

        private enum Stato
        {
            Iniziale,
            Listening,
            Connesso
        }
        private Stato _stato;

        private const int DEFAULT_PORT = 8221;

        private const int DIMBUFF = 5;

        private readonly byte[] receivedBytesBuffer = new byte[DIMBUFF];
        private string receivedString = null;

        public TcpListener _listener;
        private List<TcpClient> _clients = new List<TcpClient>();

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
            try
            {
                Log("CALL: Close(); socket Listener");

                DisconnectClients();

                _listener?.Stop();
                _listener = null;
            }
            catch (SocketException se)
            {
                MessageBox.Show(se.Message, "Server");
            }
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
            try
            {
                Log("CALL: Close(); socket Listener");

                DisconnectClients();

                _listener?.Stop();
                _listener = null;

                // Aggiorna lo stato e la UI
                _stato = Stato.Iniziale;
                UpdateLayout();
            }
            catch (SocketException se)
            {
                MessageBox.Show(se.Message, "Server");
            }
        }

        private void SendButton_Click(object sender, EventArgs e)
        {
            try
            {
                string messageText = DatiTxTextBox.Text;
                Protocol.MessageReceivedMessage messageReceivedMessage = new Protocol.MessageReceivedMessage
                {
                    username = "admin",
                    message = messageText
                };
                var messageBytes = Protocol.EncodeMessage(messageReceivedMessage.ToJson());

                foreach (var clients in _clients)
                {
                    if (clients == null)
                    {
                        MessageBox.Show("Socket di invio nullo", "Server");
                        return;
                    }

                    if (!clients.Connected)
                    {
                        MessageBox.Show("Client non connesso", "Server");
                        return;
                    }

                    Log("CALL: Send();");
                    clients.GetStream().Write(messageBytes, 0, messageBytes.Length);
                }

            }
            catch (SocketException se)
            {
                MessageBox.Show(se.Message, "Server");
            }
        }

        private void DisconnectButton_Click(object sender, EventArgs e)
        {
            //if (_sendSocket == null)
            //{
            //    MessageBox.Show("Socket di invio nullo", "Server");
            //    return;
            //}

            //if (!_sendSocket.Connected)
            //{
            //    MessageBox.Show("Client non connesso", "Server");
            //    return;
            //}

            DisconnectClients();
        }

        private void DisconnectClients()
        {
            Log("CALL: DisconnectClients(); Richiesta disconnessione");

            foreach (TcpClient client in _clients)
            {
                if (client == null)
                {
                    continue;
                }

                var stream = client.GetStream();
                stream?.Close();
                stream?.Dispose();

                client?.Close();
                client?.Dispose();
            }

            // Aggiorna lo stato e la UI
            _stato = Stato.Listening;
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

                // Aggiorna lo stato e la UI
                _stato = Stato.Connesso;
                UpdateLayout();

                Log(".........................");
                Log("CALL: BeginAccept(); BeginAccept REimpostata");
                _listener.BeginAcceptTcpClient(new AsyncCallback(OnAccept), null);

                // Invia richiesta di login
                var loginNeededMessage = new Protocol.LoginNeededMessage();
                byte[] text = Protocol.EncodeMessage(loginNeededMessage.ToJson());
                Log("Inviando richiesta di login al client");
                stream.Write(text, 0, text.Length);
            }
            catch (ObjectDisposedException)
            {
                Log("EVNT: BeginAccept(); Errore: ObjectDisposedException");
                MessageBox.Show("ObjectDisposedException", "Server");
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

            try
            {
                var client = (TcpClient)asyn.AsyncState;

                if (client == null || !client.Connected)
                {
                    Log("EVNT: OnDataReceived(); Disconnesso dal Server");

                    // Aggiorna lo stato e la UI
                    _stato = Stato.Listening;
                    UpdateLayout();

                    return;
                }

                var stream = client.GetStream();

                int numReceivedBytes = stream.EndRead(asyn);

                if (numReceivedBytes == 0)
                {
                    Log("EVNT: OnDataReceived(); Disconnesso dal Client");
                    Log("CALL: Close();");
                    client.Close();
                    client = null;

                    // Aggiorna lo stato e la UI
                    _stato = Stato.Listening;
                    UpdateLayout();

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
                    Protocol.BaseMessage message = Protocol.FromJson(maybeJsonString);

                    if (message is Protocol.LoginMessage loginMessage)
                    {
                        // Fai il login
                        ConnectionDB connection = new ConnectionDB();
                        var loginResultMessage = connection.Login(loginMessage.username, loginMessage.password);

                        // Invia il risultato
                        var bytes = Protocol.EncodeMessage(loginResultMessage.ToJson());
                        stream.Write(bytes, 0, bytes.Length);
                    }
                    else if (message is Protocol.RegisterMessage registerMessage)
                    {
                        // Fai la registrazione
                        ConnectionDB connection = new ConnectionDB();
                        var loginResultMessage = connection.Register(registerMessage.nome, registerMessage.cognome, registerMessage.username, registerMessage.password);

                        // Invia il risultato
                        var bytes = Protocol.EncodeMessage(loginResultMessage.ToJson());
                        stream.Write(bytes, 0, bytes.Length);
                    }
                    else if (message is Protocol.SendMessageMessage sendMessageMessage)
                    {
                        // Invia il messaggio agli altri client
                        var messageReceivedMessage = new Protocol.MessageReceivedMessage
                        {
                            username = "TBD",
                            message = sendMessageMessage.message
                        };
                        byte[] bytes = Protocol.EncodeMessage(messageReceivedMessage.ToJson());

                        foreach (var clients in _clients)
                        {
                            if (clients != client)
                            {
                                NetworkStream streamClient = clients.GetStream();
                                streamClient.Write(bytes, 0, bytes.Length);
                            }

                        }
                    }
                    else
                    {
                        Log("Messaggio sconosciuto ricevuto dal client");
                        Log(maybeJsonString);
                    }

                    receivedString = null;
                }

                // Torna ad ascoltare nuovi messaggi
                Log("CALL: BeginReceive(); Pronto a ricevere");
                stream.BeginRead(receivedBytesBuffer, 0, receivedBytesBuffer.Length, new AsyncCallback(OnDataReceived), client);
            }
            catch (ObjectDisposedException)
            {
                Log("EVNT: OnDataReceived(); Errore ObjectDisposedException");
                MessageBox.Show("ObjectDisposedException", "Server");
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
                    DatiTxGroupBox.Enabled = false;
                    DatiRxGroupBox.Enabled = false;
                    StartListeningButton.Enabled = false;
                    StopListeningButton.Enabled = true;
                    SendButton.Enabled = false;
                    DisconnectButton.Enabled = false;
                    break;
                case Stato.Connesso:
                    SettingsGroupBox.Enabled = false;
                    DatiTxGroupBox.Enabled = true;
                    DatiRxGroupBox.Enabled = true;
                    StartListeningButton.Enabled = false;
                    StopListeningButton.Enabled = false;
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
