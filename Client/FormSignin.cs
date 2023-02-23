using System;
using System.Windows.Forms;

namespace ChatTCP.Client
{
    public partial class FormSignin : Form
    {
        public string Nome => NomeTextBox.Text;
        public string Cognome => CognomeTextBox.Text;
        public string Email => EmailTextBox.Text;
        public string Username => UsernameTextBox.Text;
        public string Password => PasswordTextBox.Text;

        public bool Confirmed = false;

        public FormSignin()
        {
            InitializeComponent();
        }

        private void SigninButton_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(Nome))
            {
                MessageBox.Show("Nome non valido");
                return;
            }

            if (string.IsNullOrEmpty(Cognome))
            {
                MessageBox.Show("Cognome non valido");
                return;
            }

            var emailSplit = Email.Split('@');
            if (emailSplit.Length != 2 || string.IsNullOrEmpty(emailSplit[0]) || string.IsNullOrEmpty(emailSplit[1]))
            {
                MessageBox.Show("Email non valida");
                return;
            }

            if (string.IsNullOrEmpty(Username))
            {
                MessageBox.Show("Username non valido");
                return;
            }

            if (string.IsNullOrEmpty(Password))
            {
                MessageBox.Show("Password non valida");
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
