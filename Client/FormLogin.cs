using ChatTCP.Common;
using System;
using System.Windows.Forms;

namespace ChatTCP.Client
{
    public partial class FormLogin : Form
    {
        public string Username => UsernameTextBox.Text;
        public string Password => PasswordTextBox.Text;
        public string Nome = "";
        public string Cognome = "";
        public string UsernameRegister = "";
        public string PasswordRegister = "";
        public bool isRegisteredInstruction = false;

        public FormLogin()
        {
            InitializeComponent();
        }

        private void LoginButton_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(Username) || string.IsNullOrEmpty(Password))
            {
                MessageBox.Show("Dati mancanti");
                return;
            }

            Close();
        }

        private void RegisterButton_Click(object sender, EventArgs e)
        {
            FormSignin signin = new FormSignin();
            signin.ShowDialog();

            if (!signin.Confirmed)
            {
                return;
            }

            Nome = signin.Nome;
            Cognome= signin.Cognome;
            UsernameRegister = signin.Username;
            PasswordRegister = signin.Password;

            isRegisteredInstruction = true;

            Close();
        }
    }
}
