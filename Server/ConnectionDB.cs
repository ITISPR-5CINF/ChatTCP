using MySql.Data.MySqlClient;
using System.Data;
using ChatTCP.Common;
using System.Configuration;

namespace ChatTCP.Server
{
    class ConnectionDB
    {
        private static string configConn = ConfigurationManager.ConnectionStrings["MySQL"].ConnectionString;
        private readonly MySqlConnection conn = new MySqlConnection(configConn);

        public Protocol.LoginResultMessage Login(string username, string password)
        {
            Protocol.LoginResultMessage loginMessage = new Protocol.LoginResultMessage();

            string cmdText = "SELECT *" +
                " FROM utenti " +
                " WHERE utenti.username = @username AND utenti.password = @password;";

            conn.Open();

            MySqlCommand command = new MySqlCommand(cmdText, conn);
            command.Parameters.Add("@username", MySqlDbType.VarChar);
            command.Parameters.Add("@password", MySqlDbType.VarChar);
            command.Parameters["@username"].Value = username;
            command.Parameters["@password"].Value = password;
            DataTable users = new DataTable();
            users.Load(command.ExecuteReader());

            conn.Close();

            if (users.Rows.Count <= 0)
            {
                loginMessage.result = Protocol.LoginResultMessage.Result.WrongCredentials;

                return loginMessage;
            }

            if (users.Rows.Count > 1)
            {
                // what.
                loginMessage.result = Protocol.LoginResultMessage.Result.WrongCredentials;

                return loginMessage;
            }

            loginMessage.result = Protocol.LoginResultMessage.Result.Success;

            return loginMessage;
        }

        public Protocol.LoginResultMessage Register(string nome, string cognome, string email, string username, string password)
        {
            Protocol.LoginResultMessage loginMessage = new Protocol.LoginResultMessage();

            if (UserAlreadyExists(username))
            {
                loginMessage.result = Protocol.LoginResultMessage.Result.UserAlreadyExists;
                return loginMessage;
            }

            string cmdText = "INSERT INTO utenti (nome, cognome, email, username, password)" +
                " VALUES (@nome, @cognome, @email, @username, @password);";

            conn.Open();

            MySqlCommand command = new MySqlCommand(cmdText, conn);
            command.Parameters.Add("@nome", MySqlDbType.VarChar);
            command.Parameters.Add("@cognome", MySqlDbType.VarChar);
            command.Parameters.Add("@email", MySqlDbType.VarChar);
            command.Parameters.Add("@username", MySqlDbType.VarChar);
            command.Parameters.Add("@password", MySqlDbType.VarChar);
            command.Parameters["@nome"].Value = nome;
            command.Parameters["@cognome"].Value = cognome;
            command.Parameters["@email"].Value = email;
            command.Parameters["@username"].Value = username;
            command.Parameters["@password"].Value = password;

            command.ExecuteNonQuery();

            conn.Close();

            loginMessage.result = Protocol.LoginResultMessage.Result.Success;

            return loginMessage;
        }

        public void UpdateUserInfo(string username, string nome, string cognome, string email)
        {
            string cmdText = "UPDATE utenti" +
                " SET nome = @nome, cognome = @cognome, email = @email" +
                " WHERE username = @username;";

            conn.Open();

            MySqlCommand command = new MySqlCommand(cmdText, conn);
            command.Parameters.Add("@username", MySqlDbType.VarChar);
            command.Parameters.Add("@nome", MySqlDbType.VarChar);
            command.Parameters.Add("@cognome", MySqlDbType.VarChar);
            command.Parameters.Add("@email", MySqlDbType.VarChar);
            command.Parameters["@username"].Value = username;
            command.Parameters["@nome"].Value = nome;
            command.Parameters["@cognome"].Value = cognome;
            command.Parameters["@email"].Value = email;

            command.ExecuteNonQuery();

            conn.Close();

            return;
        }

        public void ChangePassword(string username, string newPassword)
        {
            string cmdText = "UPDATE utenti" +
                " SET password = @newPassword" +
                " WHERE username = @username;";

            conn.Open();

            MySqlCommand command = new MySqlCommand(cmdText, conn);
            command.Parameters.Add("@username", MySqlDbType.VarChar);
            command.Parameters.Add("@newPassword", MySqlDbType.VarChar);
            command.Parameters["@username"].Value = username;
            command.Parameters["@newPassword"].Value = newPassword;

            command.ExecuteNonQuery();

            conn.Close();

            return;
        }

        private bool UserAlreadyExists(string username)
        {
            string cmdText = "SELECT utenti.username" +
                " FROM utenti" +
                " WHERE utenti.username = @username";

            conn.Open();

            MySqlCommand command = new MySqlCommand(cmdText, conn);
            command.Parameters.Add("@username", MySqlDbType.VarChar);
            command.Parameters["@username"].Value = username;

            DataTable users = new DataTable();
            users.Load(command.ExecuteReader());

            conn.Close();

            return users.Rows.Count > 0;
        }
    }
}
