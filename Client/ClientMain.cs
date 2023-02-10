using System;
using System.Windows.Forms;

namespace ChatTCP.Client
{
    static class ClientMain
    {
        static int intLogCount = 0;
        /// <summary>
        /// Punto di ingresso principale dell'applicazione.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new ClientForm());
        }

        public static void Log(ListBox lst, string msg)
        {
            lst.Items.Insert(0, intLogCount.ToString().PadLeft(3) + " " + msg);
            intLogCount++;
        }
    }
}
