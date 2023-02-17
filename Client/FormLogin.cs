using ChatTCP.Common;
using System;
using System.Windows.Forms;

namespace ChatTCP.Client
{
    public partial class FormLogin : Form
    {
        public string Username => UsernameTextBox.Text;
        public string Password => PasswordTextBox.Text;

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

        private void buttonRegister_Click(object sender, EventArgs e)
        {
            FormSignin signin = new FormSignin();
            signin.ShowDialog();
            /*
            if (message is Protocol.RegisterMessage registerNewMessage)
            {
                var registerMessage = new Protocol.RegisterMessage
                {
                    nome = 
                };
            }*/
        }
    }
}
