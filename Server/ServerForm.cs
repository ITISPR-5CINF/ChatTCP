using System;
using System.Windows.Forms;
using System.Net; // per il tipo IPEndPoint
using System.Net.Sockets;  // per il tipo Socket

namespace ChatTCP.Server
{
    public partial class ServerForm : Form
    {


        private const string WINDOWTITLE = "Socket Server in C#";
        private const string WINDOWTITLECLIENT = "Socket Client in C#";

        enum Status
        {
            Iniziale,
            Listening,
            Connesso
        }

        Status Stato;

        private const int DIMBUFF = 5;
        private const int PORTLISTEN = 8221;
        byte[] abytRx = new byte[DIMBUFF];
        public Socket m_socListener;
        public Socket m_socWorker;






        public ServerForm()
        {
            InitializeComponent();

        }




        #region F O R M

        private void frmServer_Load(object sender, EventArgs e)
        {
            IntPtr h;

            this.Text = WINDOWTITLE;



            txtPortaTcp.Text = Convert.ToString(PORTLISTEN);

            Stato = Status.Iniziale;
            layout();




        }


        private void frmServer_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (m_socWorker != null)
            {
                if (m_socWorker.Connected)
                {
                    MessageBox.Show("Connessione aperta", "Client");
                    e.Cancel = true;
                    return;
                }
            }


            try
            {
                if (m_socListener != null)
                {
                    ServerMain.Log(lstLog, "CALL: Close(); socket Listener");
                    m_socListener.Close();
                    m_socListener = null;
                }
            }
            catch (SocketException se)
            {
                MessageBox.Show(se.Message, "Server");
            }

        }

        #endregion





        #region P U L S A N T I

        private void cmdListen_Click(object sender, EventArgs e)
        {
            string strTcp;
            int PortaTcp;
            bool blnOk;

            strTcp = txtPortaTcp.Text;
            blnOk = int.TryParse(strTcp, out PortaTcp);

            if (blnOk && PortaTcp > 1024)
            {
                try
                {
                    //Creazione del socket in attesa sulla porta nota
                    ServerMain.Log(lstLog, "-------------------------");

                    m_socListener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

                    //Associazione dell'endpoint (indirizzo IP locale/porta TCP) al socket
                    IPEndPoint ipLocal = new IPEndPoint(IPAddress.Any, PortaTcp);
                    m_socListener.Bind(ipLocal);
                    ServerMain.Log(lstLog, "CALL: Bind(); Socket creato e associato (" + IPAddress.Any.ToString() + ":" + strTcp + ")");

                    //Avvio Listening sulla porta TCP nota
                    m_socListener.Listen(4);
                    ServerMain.Log(lstLog, "CALL: Listen(); Socket in listening");

                    // Creazione funzione di callback per accettare connessioni
                    m_socListener.BeginAccept(new AsyncCallback(OnAccept), null);
                    ServerMain.Log(lstLog, "CALL: BeginAccept(); Callback BeginAccept impostata");

                    Stato = Status.Listening;
                    layout();



                }
                catch (SocketException se)
                {
                    MessageBox.Show(se.Message, "Server");
                }

            }
            else
            {
                MessageBox.Show("Numero di porta [" + txtPortaTcp.Text + "] non valido", WINDOWTITLE);
            }

        }



        private void cmdStopListening_Click(object sender, EventArgs e)
        {


            try
            {
                if (m_socListener != null)
                {
                    ServerMain.Log(lstLog, "CALL: Close(); socket Listener");
                    m_socListener.Close();
                    m_socListener = null;

                    Stato = Status.Iniziale;
                    layout();
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


        private void cmdSend_Click(object sender, EventArgs e)
        {
            try
            {
                Object objData = txtDatiTx.Text;
                byte[] byData = System.Text.Encoding.ASCII.GetBytes(objData.ToString());

                if (m_socWorker != null)
                {
                    if (m_socWorker.Connected)
                    {
                        ServerMain.Log(lstLog, "CALL: Send();");
                        m_socWorker.Send(byData);
                    }
                    else
                    {
                        MessageBox.Show("Client non connesso", "Server");
                    }
                }
                else
                {
                    MessageBox.Show("Client nullo", "Server");
                }
            }
            catch (SocketException se)
            {
                MessageBox.Show(se.Message, "Server");
            }

        }


        private void cmdDisconnect_Click(object sender, EventArgs e)
        {
            if (m_socWorker != null)
            {
                if (m_socWorker.Connected)
                {
                    ServerMain.Log(lstLog, "CALL: BeginDisconnect(); Richiesta disconnessione");
                    m_socWorker.BeginDisconnect(false, new AsyncCallback(OnDisconnect), m_socWorker);
                }
                else
                {
                    MessageBox.Show("Client non connesso", "Server");
                }
            }
            else
            {
                MessageBox.Show("Client nullo", "Server");
            }

        }



        #endregion





        #region E V E N T I

        private delegate void del_OnAccept(IAsyncResult asyn);
        public void OnAccept(IAsyncResult asyn)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new del_OnAccept(OnAccept), asyn);
                return;
            }

            if (m_socListener != null)
            {
                try
                {
                    m_socWorker = m_socListener.EndAccept(asyn);
                    ServerMain.Log(lstLog, "EVNT: BeginAccept(); Connessione accettata");

                    ServerMain.Log(lstLog, "CALL: BeginReceive(); Pronto a ricevere");
                    m_socWorker.BeginReceive(abytRx, 0, abytRx.Length, SocketFlags.None, new AsyncCallback(OnDataReceived), m_socWorker);

                    Stato = Status.Connesso;
                    layout();

                    ServerMain.Log(lstLog, ".........................");
                    ServerMain.Log(lstLog, "CALL: BeginAccept(); BeginAccept REimpostata");
                    m_socListener.BeginAccept(new AsyncCallback(OnAccept), null);

                }
                catch (ObjectDisposedException)
                {
                    ServerMain.Log(lstLog, "EVNT: BeginAccept(); Errore: ObjectDisposedException");
                    MessageBox.Show("ObjectDisposedException", "Server");
                }
                catch (SocketException se)
                {
                    ServerMain.Log(lstLog, "EVNT: BeginAccept(); Errore: " + se.Message);
                    MessageBox.Show(se.Message, "Server");
                }

            }
            else
            {
                ServerMain.Log(lstLog, "EVNT: BeginAccept(); Listener nullo");
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
                if (m_socWorker.Connected)
                {
                    int iRx = 0;
                    bool blnRemoteSocketClosing;

                    // Quanti ricevuti
                    iRx = m_socWorker.EndReceive(asyn);  //iRx = ((Socket)(asyn.AsyncState)).EndReceive(asyn);
                    blnRemoteSocketClosing = m_socWorker.Poll(1, SelectMode.SelectRead);

                    if ((!blnRemoteSocketClosing) || (iRx != 0))
                    {
                        ServerMain.Log(lstLog, "EVNT: OnDataReceived();");
                        String szData = System.Text.ASCIIEncoding.ASCII.GetString(abytRx, 0, iRx);
                        txtDatiRx.Text = txtDatiRx.Text + szData;
                        ServerMain.Log(lstLog, "CALL: BeginReceive(); Pronto a ricevere");
                        m_socWorker.BeginReceive(abytRx, 0, abytRx.Length, SocketFlags.None, new AsyncCallback(OnDataReceived), m_socWorker);
                    }
                    else
                    {
                        ServerMain.Log(lstLog, "EVNT: OnDataReceived(); Disconnesso dal Client");
                        ServerMain.Log(lstLog, "CALL: Close();");
                        m_socWorker.Close();
                        m_socWorker = null;

                        Stato = Status.Listening;
                        layout();
                    }
                }
                else
                {
                    ServerMain.Log(lstLog, "EVNT: OnDataReceived(); Disconnesso dal Server");

                    Stato = Status.Listening;
                    layout();
                }
            }

            catch (ObjectDisposedException)
            {
                ServerMain.Log(lstLog, "EVNT: OnDataReceived(); Errore ObjectDisposedException");
                MessageBox.Show("ObjectDisposedException", "Server");
            }

            catch (SocketException se)
            {
                ServerMain.Log(lstLog, "EVNT: OnDataReceived(); Errore " + se.Message);
                MessageBox.Show(se.Message, "Server");
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
            ServerMain.Log(lstLog, "EVNT: OnDisconnect(); Disconnessione");
            m_socWorker.EndDisconnect(asyn);
            ServerMain.Log(lstLog, "CALL: Close(); Socket chiuso");
            m_socWorker.Close();
        }


        #endregion





        #region R O U T I N E S 

        private void layout()
        {
            switch (Stato)
            {
                case Status.Iniziale:
                    gboxSettings.Enabled = true;
                    gboxDatiTx.Enabled = false;
                    gboxDatiRx.Enabled = false;
                    cmdListen.Enabled = true;
                    cmdStopListening.Enabled = false;
                    cmdSend.Enabled = false;
                    cmdDisconnect.Enabled = false;
                    break;

                case Status.Listening:
                    gboxSettings.Enabled = false;
                    gboxDatiTx.Enabled = false;
                    gboxDatiRx.Enabled = false;
                    cmdListen.Enabled = false;
                    cmdStopListening.Enabled = true;
                    cmdSend.Enabled = false;
                    cmdDisconnect.Enabled = false;
                    break;

                case Status.Connesso:
                    gboxSettings.Enabled = false;
                    gboxDatiTx.Enabled = true;
                    gboxDatiRx.Enabled = true;
                    cmdListen.Enabled = false;
                    cmdStopListening.Enabled = false;
                    cmdSend.Enabled = true;
                    cmdDisconnect.Enabled = true;
                    break;
            }
        }

        #endregion






    }// Classe
}//Namespace
