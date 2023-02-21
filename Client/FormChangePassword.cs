using System;
using System.Windows.Forms;

namespace ChatTCP.Client
{
    public partial class FormChangePassword : Form
    {
        public delegate void OnChangedPassword(string password);

        public FormUserInfo.OnChangePassword OnChangePasswordCallback = null;

        public FormChangePassword()
        {
            InitializeComponent();
        }

        private void ChangeButton_Click(object sender, EventArgs e)
        {
            var newPassword = NewPasswordTextBox.Text;

            if (string.IsNullOrEmpty(newPassword))
            {
                MessageBox.Show("Password non valida");
                return;
            }

            OnChangePasswordCallback?.Invoke(newPassword);

            MessageBox.Show("Password cambiata");

            Close();
        }
    }
}
