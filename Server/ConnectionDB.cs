using MySql.Data.MySqlClient;
using System.Data;
using ChatTCP.Common;

namespace ChatTCP.Server
{
    class ConnectionDB
    {
        private readonly MySqlConnection conn = new MySqlConnection("Server=localhost;Database=chat_tcp;Uid=root;Pwd=''");

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

        public Protocol.LoginResultMessage Register(string nome, string cognome, string username, string password)
        {
            Protocol.LoginResultMessage loginMessage = new Protocol.LoginResultMessage();

            if (UserAlreadyExists(username))
            {
                loginMessage.result = Protocol.LoginResultMessage.Result.UserAlreadyExists;
                return loginMessage;
            }

            string cmdText = "INSERT INTO utenti (nome, cognome, username, password)" +
                " VALUES (@nome, @cognome, @username, @password);";

            conn.Open();

            MySqlCommand command = new MySqlCommand(cmdText, conn);
            command.Parameters.Add("@nome", MySqlDbType.VarChar);
            command.Parameters.Add("@cognome", MySqlDbType.VarChar);
            command.Parameters.Add("@username", MySqlDbType.VarChar);
            command.Parameters.Add("@password", MySqlDbType.VarChar);
            command.Parameters["@nome"].Value = nome;
            command.Parameters["@cognome"].Value = cognome;
            command.Parameters["@username"].Value = username;
            command.Parameters["@password"].Value = password;

            command.ExecuteNonQuery();

            conn.Close();

            loginMessage.result = Protocol.LoginResultMessage.Result.Success;

            return loginMessage;
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
