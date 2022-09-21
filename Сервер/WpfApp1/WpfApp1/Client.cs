using ChatServer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1
{
    public class ClientObject
    {
        protected internal string Id { get; private set; }
        protected internal int nameindex = -1;
        protected internal int AdminsRights = 0;
        protected internal NetworkStream Stream { get; private set; }
        string userName;
        bool ls = false; // личные сообщения переключатель
        string idls = ""; // id человека с которым идет общение
        string namels = ""; // имя человека с которым идет общение
        TcpClient client;
        ServerObject server; // объект сервера

        public ClientObject(TcpClient tcpClient, ServerObject serverObject)
        {
            Id = Guid.NewGuid().ToString();
            
            client = tcpClient;
            server = serverObject;
            serverObject.AddConnection(this);
        }
        int CheckingForQuantity(string username, string id)
        {
            if (INFO.CountClients < INFO.k)
            {
                nameindex = INFO.InsertOnlineList(username, id);
                return nameindex;
               
            }
            return -1;
        }
        
        public void Process()
        {
            try
            {
                Stream = client.GetStream();
                // получаем имя пользователя
                string message = GetMessage();
                userName = message;
                //nameindex = INFO.InsertOnlineList(userName, this.Id); // добавление пользователя в таблицу
                if (INFO.FindID(userName) != "-1")
                {
                    server.For1Message("Данное имя уже используется", this.Id);

                }
                else if (CheckingForQuantity(userName,this.Id) == -1)
                {
                    server.For1Message("Сервер переполнен" + INFO.CountClients + "/" + INFO.k, this.Id);
                }
                else
                {
                    
                    message = userName + " вошел в чат! Добро пожаловать!";
                    // посылаем сообщение о входе в чат всем подключенным пользователям
                    server.For1Message(INFO.History, this.Id);
                    server.BroadcastMessage(message, this.Id);
                    server.For1Message(message, this.Id);
                    INFO.ADDinCHAT(message);
                    // в бесконечном цикле получаем сообщения от клиента
                    while (client.Client.Connected)
                    {
                        try
                        {
                            message = GetMessage();
                            string zero = string.Format(message);
                            if (zero == "/Exit")
                            {
                                server.RemoveConnection(this.Id);
                                //Console.WriteLine(userName + " Вышел(а) из чата");
                                
                                INFO.ADDinCHAT(userName + " Вышел(а) из чата");
                                server.BroadcastMessage(userName + " Вышел(а) из чата", this.Id);
                                break;
                            }

                            conditions(zero); //ветвление для юзеров
                            //Console.WriteLine(INFO.CompileOnlineList());
                            // server.For1Message(INFO.CompileOnlineList(), this.Id);

                            //message = String.Format("{0} -> {1}", userName, message); //
                            //Console.WriteLine(message);
                           // INFO.ADDinCHAT(message); //
                           // server.BroadcastMessage(message, this.Id); //

                        }
                        catch
                        {
                            message = String.Format("{0}: покинул чат", userName);
                            //Console.WriteLine(message);
                            INFO.ADDinCHAT(message);
                            server.BroadcastMessage(message, this.Id);
                            break;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                //Console.WriteLine(e.Message);
                INFO.ADDinCHAT(e.Message);
            }
            finally
            {
                // в случае выхода из цикла закрываем ресурсы
                if (nameindex != -1)
                { 
                INFO.DeleteOnlineList(nameindex);
                }
                server.RemoveConnection(this.Id);
                Close();
            }
            void conditions(string text) // ветвления для юзера
            {
                if (text.StartsWith("/m"))
                {
                    try
                    {
                        namels = text.Remove(0, 3);
                        idls = INFO.FindID(namels);
                        server.For1Message("теперь вы переписываетесь с " + namels, Id);
                        ls = true;
                    }
                    catch
                    {
                        server.For1Message("Ошибка", Id);
                    }

                }
                else if (ls == true && text.StartsWith("/close"))
                {
                    ls = false;
                }
                else if (text == "/cats 1")
                {
                    cats_smiles cats_Smiles = new cats_smiles();
                    server.ForAllMessage(cats_Smiles.catChoice(text, userName));

                }
                else if (text.StartsWith("/help"))
                {
                    server.For1Message("", Id);
                }
                else
                {
                    if (ls == false)
                    {
                        text = String.Format("{0} : {1}", userName, text);
                        //Console.WriteLine(message);
                        INFO.ADDinCHAT(text);
                        server.BroadcastMessage(text, this.Id);
                    }
                    else
                    {
                        try
                        {
                            server.For1Message("/m " + userName + "->" + namels + " " + text, idls);
                        }
                        catch
                        {
                            ls = false;
                            server.For1Message("ошибка в написании в лс", Id);
                        }
                    }
                }
                
            }
        }
        protected internal string Unencritp(string text) // Расшифровка текста
        {
            return text;
        }
        // чтение входящего сообщения и преобразование в строку
        private string GetMessage()
        {
            crypt crypt = new crypt();
            byte[] data = new byte[64]; // буфер для получаемых данных
            StringBuilder builder = new StringBuilder();
            int bytes = 0;
            do
            {
                bytes = Stream.Read(data, 0, data.Length);
                builder.Append(Encoding.Unicode.GetString(data, 0, bytes));
            }
            while (Stream.DataAvailable);

            return crypt.UnEncrypt(builder.ToString());
        }
        
        // закрытие подключения
        protected internal void Close()
        {
            if (Stream != null)
                Stream.Close();
            if (client != null)
                client.Close();
        }
    }
}