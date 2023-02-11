using System;
using System.Net;
using System.Net.Sockets;
using System.Windows.Forms;

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

        public Socket _receiveSocket;
        public Socket _sendSocket;

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
            if (_sendSocket != null)
            {
                if (_sendSocket.Connected)
                {
                    MessageBox.Show("Connessione aperta", "Client");
                    e.Cancel = true;
                    return;
                }
            }

            try
            {
                if (_receiveSocket != null)
                {
                    Log("CALL: Close(); socket Listener");
                    _receiveSocket.Close();
                    _receiveSocket = null;
                }
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
                _receiveSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

                // Associazione dell'endpoint (indirizzo IP locale/porta TCP) al socket
                IPEndPoint ipLocal = new IPEndPoint(IPAddress.Any, port);
                _receiveSocket.Bind(ipLocal);
                Log($"CALL: Bind(); Socket creato e associato ({IPAddress.Any}:{portString})");

                // Avvio listening sulla porta TCP nota
                _receiveSocket.Listen(4);
                Log("CALL: Listen(); Socket in listening");

                // Creazione funzione di callback per accettare connessioni
                _receiveSocket.BeginAccept(new AsyncCallback(OnAccept), null);
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
                if (_receiveSocket != null)
                {
                    Log("CALL: Close(); socket Listener");
                    _receiveSocket.Close();
                    _receiveSocket = null;

                    _stato = Stato.Iniziale;
                    UpdateLayout();
                }
                else
                {
                    MessageBox.Show("Socket listener nullo", "Server");
                }
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
                string message = DatiTxTextBox.Text;
                byte[] messageBytes = System.Text.Encoding.ASCII.GetBytes(message);
                
                if (_sendSocket == null)
                {
                    MessageBox.Show("Socket di invio nullo", "Server");
                    return;
                }

                if (!_sendSocket.Connected)
                {
                    MessageBox.Show("Client non connesso", "Server");
                    return;
                }

                Log("CALL: Send();");
                _sendSocket.Send(messageBytes);
            }
            catch (SocketException se)
            {
                MessageBox.Show(se.Message, "Server");
            }
        }

        private void DisconnectButton_Click(object sender, EventArgs e)
        {
            if (_sendSocket == null)
            {
                MessageBox.Show("Socket di invio nullo", "Server");
                return;
            }

            if (!_sendSocket.Connected)
            {
                MessageBox.Show("Client non connesso", "Server");
                return;
            }

            Log("CALL: BeginDisconnect(); Richiesta disconnessione");
            _sendSocket.BeginDisconnect(false, new AsyncCallback(OnDisconnect), _sendSocket);
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

            if (_receiveSocket == null)
            {
                Log("EVNT: BeginAccept(); Listener nullo");
                return;
            }

            try
            {
                _sendSocket = _receiveSocket.EndAccept(asyn);
                Log("EVNT: BeginAccept(); Connessione accettata");

                Log("CALL: BeginReceive(); Pronto a ricevere");
                _sendSocket.BeginReceive(receivedBytesBuffer, 0, receivedBytesBuffer.Length, SocketFlags.None, new AsyncCallback(OnDataReceived), _sendSocket);

                // Aggiorna lo stato e la UI
                _stato = Stato.Connesso;
                UpdateLayout();

                Log(".........................");
                Log("CALL: BeginAccept(); BeginAccept REimpostata");
                _receiveSocket.BeginAccept(new AsyncCallback(OnAccept), null);
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
                if (!_sendSocket.Connected)
                {
                    Log("EVNT: OnDataReceived(); Disconnesso dal Server");

                    // Aggiorna lo stato e la UI
                    _stato = Stato.Listening;
                    UpdateLayout();

                    return;
                }

                int numReceivedBytes = _sendSocket.EndReceive(asyn);
                bool isRemoteSocketClosing = _sendSocket.Poll(1, SelectMode.SelectRead);

                if (isRemoteSocketClosing && numReceivedBytes == 0)
                {
                    Log("EVNT: OnDataReceived(); Disconnesso dal Client");
                    Log("CALL: Close();");
                    _sendSocket.Close();
                    _sendSocket = null;

                    // Aggiorna lo stato e la UI
                    _stato = Stato.Listening;
                    UpdateLayout();

                    return;
                }

                Log("EVNT: OnDataReceived();");
                string szData = System.Text.Encoding.ASCII.GetString(receivedBytesBuffer, 0, numReceivedBytes);
                DatiRxTextBox.Text += szData;
                Log("CALL: BeginReceive(); Pronto a ricevere");
                _sendSocket.BeginReceive(receivedBytesBuffer, 0, receivedBytesBuffer.Length, SocketFlags.None, new AsyncCallback(OnDataReceived), _sendSocket);
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

        private delegate void del_OnDisconnect(IAsyncResult asyn);
        public void OnDisconnect(IAsyncResult asyn)
        {
            // Fa in modo che questa funzione venga eseguita nel thread corretto
            if (InvokeRequired)
            {
                BeginInvoke(new del_OnDisconnect(OnDisconnect), asyn);
                return;
            }

            Log("EVNT: OnDisconnect(); Disconnessione");
            _sendSocket.EndDisconnect(asyn);
            Log("CALL: Close(); Socket chiuso");
            _sendSocket.Close();
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
