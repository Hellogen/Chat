using System;
using System.Net.Sockets;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
namespace ChatServer
{/*
    public static class INFO // класс информации 
    {
         public static int k = 10; // количество человек в чате (ограничение) 
         public static string History = "history \n";
         public static string[] OnlineList = new string[k];
         public static bool[] checkListOnline = new bool[k];
         public static string[] ClientID = new string[k];
         public static int InsertOnlineList(string name, string id) //  добавление пользователя в таблицу с его ID.
         {
            int settedindex = 0;
            for (int i = 0; i < OnlineList.Length; i++)
            {
                if (checkListOnline[i] == true)
                {
                    OnlineList[i] = name;
                    checkListOnline[i] = false;
                    settedindex = i;
                    return settedindex;
                }
            }
            return -1; // не вошел в список
            // =^._.^= ∫ <-- кот следит за пользователями
        }
        public static void DeleteOnlineList(int index)
        {
            for (int i = 0; i < OnlineList.Length; i++)
            {
                if (index == i)
                {
                    OnlineList[i] = "";
                    checkListOnline[i] = true;
                    ClientID[i] = "";
                }
            }
        }
        public static string CompileOnlineList()
        {
            string result = "";
            int count = 0;
            for (int i = 0; i < OnlineList.Length; i++)
            {
                if (checkListOnline[i] == false)
                {
                    result += count + 1 + " " + OnlineList[i] + "\n";
                    count++;
                }
            }
            result += count + "/" + k + "\n";
            
            return result;
        }
    }
    
}
namespace ChatServer
{
    public class ClientObject
    {
        protected internal string Id { get; private set; }
        protected internal int nameindex;
        protected internal NetworkStream Stream { get; private set; }
        string userName;
        
        TcpClient client;
        ServerObject server; // объект сервера

        public ClientObject(TcpClient tcpClient, ServerObject serverObject)
        {
            Id = Guid.NewGuid().ToString();
            
            client = tcpClient;
            server = serverObject;
            serverObject.AddConnection(this);
        }

        public void Process()
        {
            try
            {
                Stream = client.GetStream();
                // получаем имя пользователя
                string message = GetMessage();
                userName = message;
                nameindex = INFO.InsertOnlineList(userName, this.Id); // добавление пользователя в таблицу
                message = userName + " вошел в чат! Добро пожаловать!";
                
                // INFO.History += message + "\n";
                // посылаем сообщение о входе в чат всем подключенным пользователям

                server.For1Message(INFO.History, this.Id);
                server.BroadcastMessage(message, this.Id);
                server.For1Message(message, this.Id);
                Console.WriteLine(message);
                Console.WriteLine(INFO.CompileOnlineList());
                //server.For1Message(INFO.CompileOnlineList(), this.Id);
                // в бесконечном цикле получаем сообщения от клиента
                while (true)
                {
                    try
                    {
                        
                        message = GetMessage();
                        string zero = string.Format(message);
                        if (zero == "/Exit")
                        {
                            server.RemoveConnection(this.Id);
                            Console.WriteLine(userName + " Вышел(а) из чата");
                            server.BroadcastMessage(userName + " Вышел(а) из чата", this.Id);   
                            
                            break;
                        }
                      
                            //Console.WriteLine(INFO.CompileOnlineList());
                           // server.For1Message(INFO.CompileOnlineList(), this.Id);

                            message = String.Format("{0} -> {1}", userName, message);
                            Console.WriteLine(message);
                        
                            server.BroadcastMessage(message, this.Id);
                        
                    }
                    catch
                    {
                        message = String.Format("{0}: покинул чат", userName);
                        Console.WriteLine(message);
                        server.BroadcastMessage(message, this.Id);
                        break;
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            finally 
            {
                // в случае выхода из цикла закрываем ресурсы
                INFO.DeleteOnlineList(nameindex);
                server.RemoveConnection(this.Id);
                Close();
            }
            void conditions(string text)
            {
               
            }
        }
        protected internal string Unencritp(string text) // Расшифровка текста
        {
            return text;
        }
        // чтение входящего сообщения и преобразование в строку
        private string GetMessage()
        {
            byte[] data = new byte[64]; // буфер для получаемых данных
            StringBuilder builder = new StringBuilder();
            int bytes = 0;
            do
            {
                bytes = Stream.Read(data, 0, data.Length);
                builder.Append(Encoding.Unicode.GetString(data, 0, bytes));
            }
            while (Stream.DataAvailable);

            return Unencritp(builder.ToString());
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
namespace ChatServer
{
    public class ServerObject
    {
        static TcpListener tcpListener; // сервер для прослушивания
        List<ClientObject> clients = new List<ClientObject>(); // все подключения

        protected internal void AddConnection(ClientObject clientObject)
        {
            clients.Add(clientObject);
        }
        protected internal void RemoveConnection(string id)
        {
            // получаем по id закрытое подключение
            ClientObject client = clients.FirstOrDefault(c => c.Id == id);
            // и удаляем его из списка подключений
            if (client != null)
                clients.Remove(client);
        }
        // прослушивание входящих подключений
        protected internal void Listen()
        {
            try
            {
                tcpListener = new TcpListener(IPAddress.Any, 8888);
                tcpListener.Start();
                Console.WriteLine("Сервер запущен. Ожидание подключений...");

                while (true)
                {
                    TcpClient tcpClient = tcpListener.AcceptTcpClient();

                    ClientObject clientObject = new ClientObject(tcpClient, this);
                    Thread clientThread = new Thread(new ThreadStart(clientObject.Process));
                    clientThread.Start();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Disconnect();
            }
        }
        // передача одному конкретному человеку
        protected internal void For1Message(string message, string id)
        {
            string FinalMessage = "";
            FinalMessage += "<Online>" + Encript(INFO.CompileOnlineList()) + "</Online>";
            FinalMessage += "<Text>" + Encript(message) + "\n" + "</Text>";
            byte[] data = Encoding.Unicode.GetBytes(FinalMessage);

            for (int i = 0; i < clients.Count; i++)
            {
                if (clients[i].Id == id) // если id клиента равно id отправляющего
                {
                    clients[i].Stream.Write(data, 0, data.Length); //передача данных
                }
            }
        }
        // трансляция сообщения подключенным клиентам без отправляющего
        protected internal void BroadcastMessage(string message, string id)
        {
            INFO.History += message + "\n";
            string FinalMessage = "";
            FinalMessage += "<Online>" + Encript(INFO.CompileOnlineList()) + "</Online>";
            FinalMessage += "<Text>"+message + "\n" + "</Text>";
            byte[] data = Encoding.Unicode.GetBytes(Encript(FinalMessage));
            for (int i = 0; i < clients.Count; i++)
            {
                if (clients[i].Id != id) // если id клиента не равно id отправляющего
                {
                    clients[i].Stream.Write(data, 0, data.Length); //передача данных
                    
                }
            }
        }

        // отключение всех клиентов
        protected internal void Disconnect()
        {
            tcpListener.Stop(); //остановка сервера

            for (int i = 0; i < clients.Count; i++)
            {
                clients[i].Close(); //отключение клиента
            }
            Environment.Exit(0); //завершение процесса
        }
        protected internal string Encript(string text) // шифрование
        { 
        return text;
        }
        
    }
}
namespace ChatServer
{
    class Program
    {
        static ServerObject server; // сервер
        static Thread listenThread; // потока для прослушивания
        static void Main(string[] args)
        {
            try
            {
            for (int i =0; i < INFO.checkListOnline.Length; i++)
                {
                    INFO.checkListOnline[i] = true;
                }
            for (int i = 0; i < INFO.OnlineList.Length; i++)
                {
                    INFO.OnlineList[i] = "";
                }
            }
            catch
            {
            }
            try
            {
                server = new ServerObject();
                listenThread = new Thread(new ThreadStart(server.Listen));
                listenThread.Start(); //старт потока
            }
            catch (Exception ex)
            {
                server.Disconnect();
                Console.WriteLine(ex.Message);
            }
        }
    } */
}
