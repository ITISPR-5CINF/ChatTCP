using System;
using System.Windows.Forms;

namespace ChatTCP.Client
{
    public partial class FormSignin : Form
    {
        public string Nome => NomeTextBox.Text;
        public string Cognome => CognomeTextBox.Text;
        public string Username => UsernameTextBox.Text;
        public string Password => PasswordTextBox.Text;

        public bool Confirmed = false;

        public FormSignin()
        {
            InitializeComponent();
        }

        private void SigninButton_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(Nome) || string.IsNullOrEmpty(Cognome) || string.IsNullOrEmpty(Username) || string.IsNullOrEmpty(Password))
            {
                MessageBox.Show("Inserisci tutti i dati");
                return;
            }

            Confirmed = true;

            Close();
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            Confirmed = false;

            Close();
        }
    }
}
