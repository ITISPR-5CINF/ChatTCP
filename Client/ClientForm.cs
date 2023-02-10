using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Windows.Forms;

namespace ChatTCP.Client
{
    public partial class ClientForm : Form
    {
        private const string WINDOWTITLE = "Socket Client in C#";

        private enum Stato
        {
            Disconnesso,
            Connesso
        }
        private Stato _stato;

        private const int DEFAULT_PORT = 8221;

        private const int DIMBUFF = 5;

        private readonly byte[] receivedBytesBuffer = new byte[DIMBUFF];

        private Socket _clientSocket;

        public ClientForm()
        {
            InitializeComponent();
        }

        private void ClientForm_Load(object sender, EventArgs e)
        {
            // Imposta il titolo della finestra
            Text = WINDOWTITLE;

            // Imposta porta di default
            PortaTcpTextBox.Text = Convert.ToString(DEFAULT_PORT);

            // Ricavo tutti i nomi dei pc in rete
            List<string> NetComputers = DLLWrapper_Netapi32.GetNetworkComputers();
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
            if (_clientSocket != null)
            {
                if (_clientSocket.Connected)
                {
                    MessageBox.Show("Connessione aperta", "Client");
                    e.Cancel = true;
                }
            }
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
                Log("...........................");
                _clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

                // Creazione dell'endpoint 
                IPEndPoint ipRemote = new IPEndPoint(rawIpAddress, port);
                Log("CALL: Socket creato (" + ipAddressString + ":" + portString + ")");

                // Associazione dell'endpoint (indirizzo IP locale/porta TCP) al socket
                _clientSocket.BeginConnect(ipRemote, new AsyncCallback(OnConnect), null);
                Log("CALL: BeginConnect(); Richiesta connessione");
            }
            catch (SocketException se)
            {
                MessageBox.Show(se.Message, "Client");
            }
        }

        private void CloseButton_Click(object sender, EventArgs e)
        {
            if (_clientSocket == null)
            {
                MessageBox.Show("Client nullo", "Client");
                return;
            }

            if (!_clientSocket.Connected)
            {
                MessageBox.Show("Client non connesso", "Client");
                return;
            }

            Log("CALL: BeginDisconnect(); Richiesta disconnessione");
            _clientSocket.BeginDisconnect(false, new AsyncCallback(OnDisconnect), _clientSocket);
        }

        private void SendButton_Click(object sender, EventArgs e)
        {
            try
            {
                string message = DatiTxTextBox.Text;
                byte[] messageBuffer = System.Text.Encoding.ASCII.GetBytes(message);

                if (_clientSocket == null)
                {
                    MessageBox.Show("Client nullo", "Client");
                    return;
                }

                if (!_clientSocket.Connected)
                {
                    MessageBox.Show("Client non connesso", "Client");
                    return;
                }

                Log("CALL: Send();");
                _clientSocket.Send(messageBuffer);
            }
            catch (SocketException se)
            {
                MessageBox.Show(se.Message, "Client");
            }
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
                return;
            }

            _clientSocket.EndConnect(asyn);
            Log("EVNT: OnConnect(); Connessione accettata");
            Log("CALL: BeginReceive(); Pronto a ricevere");
            _clientSocket.BeginReceive(receivedBytesBuffer, 0, receivedBytesBuffer.Length, SocketFlags.None, new AsyncCallback(OnDataReceived), _clientSocket);

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
                if (!_clientSocket.Connected)
                {
                    Log("EVNT: OnDataReceived(); Disconnesso dal Client");
                    return;
                }

                int numReceivedBytes = _clientSocket.EndReceive(asyn);
                bool isRemoteSocketClosing = _clientSocket.Poll(1, SelectMode.SelectRead);

                if (isRemoteSocketClosing && numReceivedBytes == 0)
                {
                    Log("EVNT: OnDataReceived(); Disconnesso dal Server");
                    Log("CALL: Close();");
                    _clientSocket.Close();
                    _clientSocket = null;

                    // Aggiorna lo stato e la UI
                    _stato = Stato.Disconnesso;
                    AggiornaLayout();
                    return;
                }

                Log("EVNT: OnDataReceived();");
                string receivedMessage = System.Text.Encoding.ASCII.GetString(receivedBytesBuffer, 0, numReceivedBytes);
                DatiRxTextBox.Text += receivedMessage;
                Log("CALL: BeginReceive(); Pronto a ricevere");
                _clientSocket.BeginReceive(receivedBytesBuffer, 0, receivedBytesBuffer.Length, SocketFlags.None, new AsyncCallback(OnDataReceived), _clientSocket);
            }
            catch (ObjectDisposedException)
            {
                Log("EVNT: OnDataReceived(); Errore ObjectDisposedException");
                MessageBox.Show("ObjectDisposedException", "Client");
            }
            catch (SocketException se)
            {
                Log("EVNT: OnDataReceived(); Errore " + se.Message);
                MessageBox.Show(se.Message, "Client");
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

            Log("EVNT: OnDisconnect();");
            _clientSocket.EndDisconnect(asyn);

            Log("CALL: Shutdown();");
            _clientSocket.Shutdown(SocketShutdown.Both);
            Log("CALL: Close();");
            _clientSocket.Close();
            //m_socClient = null; //genera una eccezione globale

            // Aggiorna lo stato e la UI
            _stato = Stato.Disconnesso;
            AggiornaLayout();
        }

        private void AggiornaLayout()
        {
            switch (_stato)
            {
                case Stato.Disconnesso:
                    ImpostazioniGroupBox.Enabled = true;
                    DatiTxGroupBox.Enabled = false;
                    DatiRxGroupBox.Enabled = false;
                    ConnectButton.Enabled = true;
                    CloseButton.Enabled = false;
                    SendButton.Enabled = false;
                    break;
                case Stato.Connesso:
                    ImpostazioniGroupBox.Enabled = false;
                    DatiTxGroupBox.Enabled = true;
                    DatiRxGroupBox.Enabled = true;
                    ConnectButton.Enabled = false;
                    CloseButton.Enabled = true;
                    SendButton.Enabled = true;
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

        private int intLogCount = 0;
        private void Log(string message)
        {
            LogListBox.Items.Add(intLogCount.ToString().PadLeft(3) + " " + message);
            LogListBox.SelectedIndex = LogListBox.Items.Count - 1;
            intLogCount++;
        }
    }
}
