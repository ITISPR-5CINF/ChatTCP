using System;
using System.Linq;
using System.Text;
using System.Text.Json;

namespace ChatTCP.Common
{
    public static class Protocol
    {
        /// <summary>
        /// Classe di base che rappresenta un messaggio
        /// </summary>
        public class BaseMessage
        {
            virtual public string action { get; set; }

            virtual public string ToJson()
            {
                return JsonSerializer.Serialize(this);
            }
        }

        /// <summary>
        /// Messaggio che rappresenta una richiesta di login del server verso un client
        /// </summary>
        public class LoginNeededMessage : BaseMessage
        {
            override public string action => "login_needed";

            override public string ToJson()
            {
                return JsonSerializer.Serialize(this);
            }
        }

        /// <summary>
        /// Messaggio che rappresenta un tentativo di login da parte di un client
        /// </summary>
        public class LoginMessage : BaseMessage
        {
            override public string action => "login";

            public string username { get; set; }
            public string password { get; set; }

            override public string ToJson()
            {
                return JsonSerializer.Serialize(this);
            }
        }

        /// <summary>
        /// Messaggio che rappresenta un tentativo di registrazione da parte di un client
        /// </summary>
        public class RegisterMessage : BaseMessage
        {
            override public string action => "login";

            public string username { get; set; }
            public string password { get; set; }
            public string nome { get; set; }
            public string cognome { get; set; }

            override public string ToJson()
            {
                return JsonSerializer.Serialize(this);
            }
        }

        /// <summary>
        /// Messaggio che rappresenta l'esito di un tentativo di login o registrazione
        /// </summary>
        public class LoginResultMessage : BaseMessage
        {
            override public string action => "login_result";

            public enum Result
            {
                Success,
                WrongCredentials,
                UserAlreadyExists,
            }

            public Result result { get; set; }

            override public string ToJson()
            {
                return JsonSerializer.Serialize(this);
            }
        }

        /// <summary>
        /// Messaggio che rappresenta l'invio di un messaggio da parte di un client verso un server
        /// </summary>
        public class SendMessageMessage : BaseMessage
        {
            override public string action => "send_message";

            public string message { get; set; }

            override public string ToJson()
            {
                return JsonSerializer.Serialize(this);
            }
        }

        /// <summary>
        /// Messaggio che rappresenta l'invio di un messaggio da parte di un client
        /// </summary>
        public class MessageReceivedMessage : BaseMessage
        {
            override public string action => "message_received";

            public string username { get; set; }
            public string message { get; set; }

            override public string ToJson()
            {
                return JsonSerializer.Serialize(this);
            }
        }

        /// <summary>
        /// Converti una stringa JSON in oggetto
        /// </summary>
        /// <param name="json">Il messaggio ricevuto</param>
        /// <returns>Un oggetto di una classe che eredita <c>BaseMessage</c></returns>
        public static BaseMessage FromJson(string json)
        {
            if (json == null) {
                return null;
            }

            try
            {
                var baseMessage = JsonSerializer.Deserialize<BaseMessage>(json);

                switch (baseMessage.action)
                {
                    case "login_needed":
                        return JsonSerializer.Deserialize<LoginNeededMessage>(json);
                    case "login":
                        return JsonSerializer.Deserialize<LoginMessage>(json);
                    case "register":
                        return JsonSerializer.Deserialize<RegisterMessage>(json);
                    case "login_result":
                        return JsonSerializer.Deserialize<LoginResultMessage>(json);
                    case "send_message":
                        return JsonSerializer.Deserialize<SendMessageMessage>(json);
                    case "message_received":
                        return JsonSerializer.Deserialize<MessageReceivedMessage>(json);
                    default:
                        // throw new Exception("Unknown action");
                        return null;
                }
            }
            catch (Exception e)
            {
                throw e;
                //return null;
            }
        }

        public static byte[] EncodeMessage(string s)
        {
            if (s == null)
            {
                return null;
            }

            if (s.Contains('\0'))
            {
                throw new Exception("Message can't contain NUL");
            }

            return Encoding.UTF8.GetBytes(s + '\0');
        }

        public static string DecodeMessage(byte[] bytes, int numBytes)
        {
            if (bytes == null || numBytes < 0)
            {
                return null;
            }

            return Encoding.UTF8.GetString(bytes, 0, numBytes);
        }

        public static string GetMessageOrNull(string s)
        {
            if (string.IsNullOrEmpty(s))
            {
                return null;
            }

            if (s.Substring(s.Length - 1) != "\0")
            {
                return null;
            }

            return s.Substring(0, s.Length - 1);
        }
    }
}
