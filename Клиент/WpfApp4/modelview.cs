using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace WpfApp4
{
    class modelview : INotifyPropertyChanged // MVVM
    {
        public event PropertyChangedEventHandler PropertyChanged;
        
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
        bool debugToPath = false; // off - false (отключает дебаг) / true
        /// <summary>
        /// 
        /// </summary>
        /// 
        private InfoCom InfoCom = new InfoCom();
 
        private string chat = "";
        private string message = "";
        private string onlinelist = "";
        public string Chat
        {
            get { return chat; }
            set { chat = value; OnPropertyChanged("Chat"); }
        }
        public string Message
        {
            get { return message; }
            set { message = value; OnPropertyChanged("Message"); }
        }
        public string OnlineList
        {
            get { return onlinelist; }
            set { onlinelist = value; OnPropertyChanged("OnlineList"); }
        }
        void Init_tcp()
        {
            TCP_SOCKET.client = new TcpClient(); // client = new TcpClient();
            try
            {
                InfoCom.logging("INITIALIZE");
                if (debugToPath == true) // дебаг
                {
                    TCP_SOCKET.CHAT += "INITIALIZE" + InfoBank.path_to_log +"\n";
                    TCP_SOCKET.CHAT += chat.Length.ToString() + "\n";
                }
                else
                {
                    TCP_SOCKET.CHAT += "INITIALIZE" + "\n";
                    
                }
                TCP_SOCKET.client.Connect(TCP_SOCKET.host, TCP_SOCKET.port ); //client.Connect(host, port); //подключение клиента
                TCP_SOCKET.stream = TCP_SOCKET.client.GetStream();// stream = client.GetStream(); // получаем поток
                InfoCom.logging("was connected");
                TCP_SOCKET.SendNoshowingMessage(TCP_SOCKET.userName);

                // запускаем новый поток для получения данных
                TCP_SOCKET.receiveThread.Start(); //старт потока
                //TCP_SOCKET.CHAT += "добро пожаловать! " + TCP_SOCKET.userName + "\n";//Console.WriteLine("Добро пожаловать, {0}", userName);
            }
            catch (Exception ex)
            {
                InfoCom.logging("error with initialize: " + ex.Message );
            }
        }
        
        public modelview()
        {
            Task.Factory.StartNew(() =>
            {
                Init_tcp();
                while (true)
                {
                    if (chat.Length == 10000)
                    {
                        
                    }
                    Chat = TCP_SOCKET.CHAT;
                    OnlineList = InfoBank.ListOnline;
                    Task.Delay(1000).Wait();
                    
                }
                
            });
        }
    }
}
