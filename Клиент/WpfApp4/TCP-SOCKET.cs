using System;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace WpfApp4
{
    public static class TCP_SOCKET
    {
        public static Thread receiveThread = new Thread(new ThreadStart(TCP_SOCKET.ReceiveMessage));
        public static string userName = "test";
        public static string host = "127.0.0.1";
        public static int port = 8888;
        public static TcpClient client;
        public static NetworkStream stream;
        public static string CHAT = "";
        public static bool IsWorking = false;
        
        public static void SendMessage(string text) // отправить сообщение
        {
            Encrypt encrypt = new Encrypt();
            try
            {
                string message = (text);
                byte[] data = Encoding.Unicode.GetBytes(encrypt.Encrypting(message));
                if (client.Connected == true)
                {
                    stream.Write(data, 0, data.Length);
                }
                CHAT += userName + ": " + message + "\n";
                InfoCom infoCom = new InfoCom();
                infoCom.logging(userName + ": " + message + "\n");
            }
            catch (Exception ex)
            {
                InfoCom infoCom = new InfoCom();
                infoCom.logging("Error with send message: " + ex.Message);
            }
        }
        public static void SendNoshowingMessage(string text) // отправить сообщение без добавления его в чат
        {
            Encrypt encrypt = new Encrypt();
            string message = encrypt.Encrypting(text);
            byte[] data = Encoding.Unicode.GetBytes(message);
            stream.Write(data, 0, data.Length);
          //  CHAT += userName + ": " + message + "\n";
        }
        public static void ReceiveMessage()
        {
            
            InfoCom infoCom = new InfoCom();
            while (client.Connected)
            {
                try
                {
                    byte[] data = new byte[64]; // буфер для получаемых данных
                    StringBuilder builder = new StringBuilder();
                    int bytes = 0;
                    do
                    {
                        bytes = stream.Read(data, 0, data.Length);
                        builder.Append(Encoding.Unicode.GetString(data, 0, bytes));
                    }
                    while (stream.DataAvailable);

                    string message = builder.ToString();
                    Encrypt encrypt = new Encrypt();
                    condition(encrypt.Unencrypting(message)); // ветвление
                    //CHAT += message + "\n"; // Console.WriteLine(message);//вывод сообщения дебаг после отключения condition(); пожалуйста отключите если включите conditions
                    //infoCom.logging(message);
                }
                catch (Exception ex)
                {
                    
                    CHAT += "Подключение прервано\n"; //Console.WriteLine("Подключение прервано!"); //соединение было прервано
                    infoCom.logging("lost signal: " + ex.Message);
                    InfoBank.error_message = "Lost signal: " +ex.Message;
                    Disconnect();
                    
                    
                }
            }
        }
        public static void Disconnect()
        {
            InfoCom infoCom = new InfoCom();
            client.Close();
            infoCom.logging("exit");
            
            receiveThread.Abort();
            Environment.Exit(0);
        }
        public static void DisconnectEx()
        {
            InfoCom infoCom = new InfoCom();
            infoCom.logging("exit");
            receiveThread.Abort();
            client.Close();
            Environment.Exit(0);
        }
        public static void ClearChat()
        {
            DateTime now = DateTime.Now;
            CHAT = "Чат был очищен: " + now.ToString() + "\n";
        }
        static string condition(string text)
        {
            
            /// Заголовки: <online></online>
            /// <Text></Text> - должен быть последним
            ///
            ///
            commands.ForCode forCode = new commands.ForCode();
            Encrypt Encrypt = new Encrypt();
            string[] header = new string[4] {"<Online>", "</Online>" , "<Text>" , "</Text>"}; // заголовок, обычно снала идет <online> и перебирание массива до следующего заголовка
            string[] splitedtext = text.Split(header, StringSplitOptions.RemoveEmptyEntries);
            string OnlineList = "";
            
            InfoCom infoCom = new InfoCom();
            if (true)
            {
               OnlineList = splitedtext[0];
                InfoBank.ListOnline = OnlineList;
                for (int i = 0; i < splitedtext.Length; i++)
                {
                    if (i % 2 == 1) // каждое второе под заголовком текст
                    {
                        
                        CHAT += splitedtext[i];
                    }
                        
                }
                //CHAT += NewText + "\n";
                
               //CHAT += text + "\n";
               // CHAT += InfoBank.ListOnline;

            }
            else
            {
                //CHAT += text + "\n"; // Console.WriteLine(message);//вывод сообщения
                infoCom.logging(text);
            }
            return "";
        }
        
    }
}
