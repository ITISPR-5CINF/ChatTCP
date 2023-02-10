using System;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;

using System.Collections;

namespace ChatTCP.Client
{
    public partial class ClientForm : Form
    {

        private const string WINDOWTITLE = "Socket Client in C#";
        private const string WINDOWTITLESERVER = "Socket Server in C#";

        enum Status
        {
            Iniziale,
            Connesso
        }

        Status Stato;

        //private const string IPREMOTE = "127.0.0.1";
        private const string IPREMOTE = "localhost";
        string strIpDotted;

        //private const string IPREMOTE = "DellE4310";
        private const int PORTLISTEN = 8221;
        private const int DIMBUFF = 5;
        byte[] abytRx = new byte[DIMBUFF];

        public Socket m_socClient;


        public ClientForm()
        {
            InitializeComponent();
        }



        #region F O R M

        private void frmClient_Load(object sender, EventArgs e)
        {
            IntPtr h;
            ArrayList NetComputers;

            this.Text = WINDOWTITLE;

            h = (IntPtr)clsWin32_Windows.FindWindow(null, WINDOWTITLESERVER);
            if (h != IntPtr.Zero)
            {
                var rect = new clsWin32_Windows.RECT();
                clsWin32_Windows.GetWindowRect(h, out rect);
                this.Left = rect.Left + (rect.Right - rect.Left) + 10;
                this.Top = rect.Top;
            }

            txtIPRemoto.Text = IPREMOTE;
            txtPortaTcp.Text = Convert.ToString(PORTLISTEN);

            // Ricavo tutti i nomi dei pc in rete
            NetComputers = clsWin32_Network.GetNetworkComputers();
            // Aggiungo il computer locale
            NetComputers.Insert(0, "localhost");
            // Li inserisco nel combo
            for (int i = 0; i < NetComputers.Count; i++)
            {
                cmbNetworkComputers.Items.Add(NetComputers[i]);
            }
            // Seleziono localhost di default
            cmbNetworkComputers.SelectedIndex = 0;
            cmbNetworkComputers.Text = (string)cmbNetworkComputers.Items[cmbNetworkComputers.SelectedIndex];

            Stato = Status.Iniziale;
            layout();

        }

        private void frmClient_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (m_socClient != null)
            {
                if (m_socClient.Connected)
                {
                    MessageBox.Show("Connessione aperta", "Client");
                    e.Cancel = true;
                }
            }
        }

        #endregion




        #region P U L S A N T I

        private void cmdConnect_Click(object sender, EventArgs e)
        {
            string strIp;
            string strTcp;
            int PortaTcp;
            long IndirizzoIp;
            bool blnOkIp;
            bool blnOk;



            strIp = strIpDotted;
            ;
            strTcp = txtPortaTcp.Text;

            //strIp = txtIPRemoto.Text;


            IsLocalIpV4Address(strIp, ref strIp);
            blnOkIp = ControllaIndirizzoIpV4(strIp);
            blnOk = int.TryParse(strTcp, out PortaTcp);

            if (blnOkIp && blnOk && PortaTcp > 1024)
            {
                IPAddress ip;

                ip = IPAddress.Parse(strIp);
                IndirizzoIp = (long)(uint)BitConverter.ToInt32(ip.GetAddressBytes(), 0);

                try
                {
                    // Creazione del socket
                    ClientMain.Log(lstLog, "...........................");
                    m_socClient = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

                    // Creazione dell'endpoint 
                    IPEndPoint ipRemote = new IPEndPoint(IndirizzoIp, PortaTcp);
                    ClientMain.Log(lstLog, "CALL: Socket creato (" + strIp + ":" + strTcp + ")");

                    // Associazione dell'endpoint (indirizzo IP locale/porta TCP) al socket
                    m_socClient.BeginConnect(ipRemote, new AsyncCallback(OnConnect), null);
                    ClientMain.Log(lstLog, "CALL: BeginConnect(); Richiesta connessione");

                }
                catch (SocketException se)
                {
                    MessageBox.Show(se.Message, "Client");
                }
            }
            else
            {
                MessageBox.Show("Numero di porta [" + txtPortaTcp.Text + "]\no Indirizzo Ip [" + txtIPRemoto.Text + "] non valido", WINDOWTITLE);
            }


        }


        private void cmdClose_Click(object sender, EventArgs e)
        {
            if (m_socClient != null)
            {
                if (m_socClient.Connected)
                {
                    ClientMain.Log(lstLog, "CALL: BeginDisconnect(); Richiesta disconnessione");
                    m_socClient.BeginDisconnect(false, new AsyncCallback(OnDisconnect), m_socClient);
                }
                else
                {
                    MessageBox.Show("Client non connesso", "Client");
                }

            }
            else
            {
                MessageBox.Show("Client nullo", "Client");
            }
        }




        private void cmdSend_Click(object sender, EventArgs e)
        {
            try
            {
                Object objData = txtDatiTx.Text;
                byte[] byData = System.Text.Encoding.ASCII.GetBytes(objData.ToString());
                if (m_socClient != null)
                {
                    if (m_socClient.Connected)
                    {
                        ClientMain.Log(lstLog, "CALL: Send();");
                        m_socClient.Send(byData);
                    }
                    else
                    {
                        MessageBox.Show("Client non connesso", "Client");
                    }
                }
                else
                {
                    MessageBox.Show("Client nullo", "Client");
                }
            }
            catch (SocketException se)
            {
                MessageBox.Show(se.Message, "Client");
            }

        }




        #endregion





        #region E V E N T I

        private delegate void del_OnConnect(IAsyncResult asyn);
        public void OnConnect(IAsyncResult asyn)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new del_OnConnect(OnConnect), asyn);
                return;
            }

            if (asyn.IsCompleted)
            {
                if (m_socClient.Connected)
                {
                    m_socClient.EndConnect(asyn);
                    ClientMain.Log(lstLog, "EVNT: OnConnect(); Connessione accettata");
                    ClientMain.Log(lstLog, "CALL: BeginReceive(); Pronto a ricevere");
                    m_socClient.BeginReceive(abytRx, 0, abytRx.Length, SocketFlags.None, new AsyncCallback(OnDataReceived), m_socClient);

                    Stato = Status.Connesso;
                    layout();

                }
                else
                {
                    ClientMain.Log(lstLog, "EVNT: OnConnect(); Connessione NON accettata");
                    MessageBox.Show("Impossibile connettersi", "Client");
                }
            }

        }



        private delegate void del_OnDataReceived(IAsyncResult asyn);
        public void OnDataReceived(IAsyncResult asyn)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new del_OnDataReceived(OnDataReceived), asyn);
                return;
            }

            try
            {

                if (m_socClient.Connected)
                {
                    int iRx = 0;
                    bool blnRemoteSocketClosing;

                    // Quanti ricevuti
                    iRx = m_socClient.EndReceive(asyn);  //iRx = ((Socket)(asyn.AsyncState)).EndReceive(asyn);
                    blnRemoteSocketClosing = m_socClient.Poll(1, SelectMode.SelectRead);

                    if ((!blnRemoteSocketClosing) || (iRx != 0))
                    {
                        ClientMain.Log(lstLog, "EVNT: OnDataReceived();");
                        String szData = System.Text.ASCIIEncoding.ASCII.GetString(abytRx, 0, iRx);
                        txtDatiRx.Text = txtDatiRx.Text + szData;
                        ClientMain.Log(lstLog, "CALL: BeginReceive(); Pronto a ricevere");
                        m_socClient.BeginReceive(abytRx, 0, abytRx.Length, SocketFlags.None, new AsyncCallback(OnDataReceived), m_socClient);
                    }
                    else
                    {
                        ClientMain.Log(lstLog, "EVNT: OnDataReceived(); Disconnesso dal Server");
                        ClientMain.Log(lstLog, "CALL: Close();");
                        m_socClient.Close();
                        m_socClient = null;

                        Stato = Status.Iniziale;
                        layout();

                    }
                }
                else
                {
                    ClientMain.Log(lstLog, "EVNT: OnDataReceived(); Disconnesso dal Client");
                }
            }

            catch (ObjectDisposedException)
            {
                ClientMain.Log(lstLog, "EVNT: OnDataReceived(); Errore ObjectDisposedException");
                MessageBox.Show("ObjectDisposedException", "Client");
            }

            catch (SocketException se)
            {
                ClientMain.Log(lstLog, "EVNT: OnDataReceived(); Errore " + se.Message);
                MessageBox.Show(se.Message, "Client");
            }
        }



        private delegate void del_OnDisconnect(IAsyncResult asyn);
        public void OnDisconnect(IAsyncResult asyn)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new del_OnDisconnect(OnDisconnect), asyn);
                return;
            }
            ClientMain.Log(lstLog, "EVNT: OnDisconnect();");
            m_socClient.EndDisconnect(asyn);

            ClientMain.Log(lstLog, "CALL: Shutdown();");
            m_socClient.Shutdown(SocketShutdown.Both);
            ClientMain.Log(lstLog, "CALL: Close();");
            m_socClient.Close();
            //m_socClient = null; //genera una eccezione globale

            Stato = Status.Iniziale;
            layout();

        }



        #endregion





        #region R O U T I N E S

        private void layout()
        {
            switch (Stato)
            {
                case Status.Iniziale:
                    gboxImpostazioni.Enabled = true;
                    gboxDatiTx.Enabled = false;
                    gboxDatiRx.Enabled = false;
                    cmdConnect.Enabled = true;
                    cmdClose.Enabled = false;
                    cmdSend.Enabled = false;
                    break;


                case Status.Connesso:
                    gboxImpostazioni.Enabled = false;
                    gboxDatiTx.Enabled = true;
                    gboxDatiRx.Enabled = true;
                    cmdConnect.Enabled = false;
                    cmdClose.Enabled = true;
                    cmdSend.Enabled = true;
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
            string hostName;

            hostName = strHost;


            if (strHost.Contains(":"))
            {
                string[] hostParts = strHost.Split(':');

                if (hostParts.Length == 2)
                {
                    int port;
                    hostName = hostParts[0];
                    return int.TryParse(hostParts[1], out port);
                }
            }
            else
            {
                if (strHost.Contains("."))
                {
                    string[] hostDottedParts = strHost.Split('.');
                    if (hostDottedParts.Length == 4)
                    {
                        int i;
                        int j = 0;
                        byte dot;
                        for (i = 0; i < hostDottedParts.Length; i++)
                        {
                            if (byte.TryParse(hostDottedParts[i], out dot))
                            {
                                j++;
                            }
                        }
                        if (j == hostDottedParts.Length) return true;
                    }
                }
            }
            return false;
        }

        #endregion

        private void cmbNetworkComputers_SelectedIndexChanged(object sender, EventArgs e)
        {
            string strComputerName;
            int idx;
            int i;




            strComputerName = cmbNetworkComputers.SelectedItem.ToString();

            idx = cmbNetworkComputers.SelectedIndex;
            strComputerName = (string)cmbNetworkComputers.Items[idx];

            //ComputerName = Dns.GetHostName();
            IPAddress[] IPs;
            strIpDotted = "";
            IPs = Dns.GetHostAddresses(strComputerName);
            if (IPs.Length > 0)
            {
                for (i = 0; i < IPs.Length; i++)
                {
                    if (IPs[i].AddressFamily == AddressFamily.InterNetwork)
                    {
                        strIpDotted = IPs[i].ToString();
                        txtIPRemoto.Text = strIpDotted;
                        break;
                    }
                }
            }




            if (strIpDotted != "")
            {
                IPAddress hostIPAddress;
                IPHostEntry hostInfo;

                hostIPAddress = IPAddress.Parse(strIpDotted); //hostInfo = Dns.GetHostByAddress(hostIPAddress);
                hostInfo = Dns.GetHostEntry(hostIPAddress);
                strComputerName = hostInfo.HostName;
                txtIPRemoto.Text = strIpDotted + ";" + strComputerName;
            }



        }




    }// Classe
}// Namespace
