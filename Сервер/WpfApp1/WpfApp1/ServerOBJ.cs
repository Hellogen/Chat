using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace WpfApp1
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
            {   //INFO.ADDinCHAT("Сервер запущен. Ожидание подключений...");
                
                tcpListener = new TcpListener(IPAddress.Any, 8888);
                tcpListener.Start();
                //Console.WriteLine("Сервер запущен. Ожидание подключений...");
                INFO.ADDinCHAT("Сервер запущен, Ожидайте подключений");
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
                INFO.ADDinCHAT(ex.Message);
                Disconnect();
            }
        }
        // передача одному конкретному человеку
        protected internal void For1Message(string message, string id)
        {
            crypt crypt = new crypt();
            string FinalMessage = "";
            FinalMessage += "<Online>" + INFO.CompileOnlineList() + "</Online>";
            FinalMessage += "<Text>" + message + "\n" + "</Text>";
            byte[] data = Encoding.Unicode.GetBytes(crypt.Encrypt(FinalMessage));

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
            crypt crypt = new crypt();
            INFO.History += message + "\n";
            string FinalMessage = "";
            FinalMessage += "<Online>" + INFO.CompileOnlineList() + "</Online>";
            FinalMessage += "<Text>" + message + "\n" + "</Text>";
            byte[] data = Encoding.Unicode.GetBytes(crypt.Encrypt(FinalMessage));
            for (int i = 0; i < clients.Count; i++)
            {
                if (clients[i].Id != id) // если id клиента не равно id отправляющего
                {
                    clients[i].Stream.Write(data, 0, data.Length); //передача данных
                }
            }
        }
        protected internal void ForAllMessage(string message) // для всех
        {
            crypt crypt = new crypt();
            INFO.History += message + "\n";
            string FinalMessage = "";
            FinalMessage += "<Online>" + INFO.CompileOnlineList() + "</Online>";
            FinalMessage += "<Text>" + (message) + "\n" + "</Text>";
            byte[] data = Encoding.Unicode.GetBytes(crypt.Encrypt(FinalMessage));
            for (int i = 0; i < clients.Count; i++)
            {    
                    clients[i].Stream.Write(data, 0, data.Length); //передача данных
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
        public void kick(string name, string reason = "вы были кикнуты")
        {
            int appruve = 0;
                string id = INFO.FindID(name);
                for (int i = 0; i < clients.Count; i++)
                {
                    if (clients[i].Id == id)
                    {
                    appruve++;
                        For1Message("Вы были кикнуты", id);
                        clients[i].Close();
                    }

                }
                if (appruve > 0)
                {
                     BroadcastMessage("Причина: " + reason,id);
                }
                else
                {
                     INFO.CHAT += "не обнаружен";
                }
        }
        protected internal string Encript(string text) // шифрование
        {
            return text;
        }
    }
}
