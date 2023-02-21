using System;
using System.Windows.Forms;

namespace ChatTCP.Client
{
    public partial class FormUserInfo : Form
    {
        public delegate void OnUpdatedUserInfo(string nome, string cognome);
        public OnUpdatedUserInfo OnUpdateUserInfoCallback = null;

        public delegate void OnChangePassword(string password);
        public OnChangePassword OnChangePasswordCallback = null;

        private readonly string username;
        private string nome;
        private string cognome;

        public FormUserInfo(string username, string nome, string cognome)
        {
            this.username = username;
            this.nome = nome;
            this.cognome = cognome;

            InitializeComponent();
        }

        private void FormUserInfo_Load(object sender, EventArgs e)
        {
            if (username != null)
            {
                UsernameLabel.Text = $"Username: {username}";
            }

            if (nome != null)
            {
                NomeTextBox.Text = nome;
            }

            if (cognome != null)
            {
                CognomeTextBox.Text = cognome;
            }
        }

        private void ChangePasswordButton_Click(object sender, EventArgs e)
        {
            var form = new FormChangePassword
            {
                OnChangePasswordCallback = OnChangePasswordCallback
            };
            form.ShowDialog();
        }

        private void UpdateUserInfoButton_Click(object sender, EventArgs e)
        {
            nome = NomeTextBox.Text;
            cognome = CognomeTextBox.Text;

            if (string.IsNullOrEmpty(nome))
            {
                MessageBox.Show("Nome non valido");
                return;
            }

            if (string.IsNullOrEmpty(cognome))
            {
                MessageBox.Show("Cognome non valido");
                return;
            }

            OnUpdateUserInfoCallback?.Invoke(nome, cognome);

            MessageBox.Show("Informazioni salvate");
        }
    }
}
