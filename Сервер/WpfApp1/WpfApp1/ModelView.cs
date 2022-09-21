using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace WpfApp1
{
    class modelview : INotifyPropertyChanged // MVVM
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
        private string chat = "";
        public string Chat
        {
            get { return chat; }
            set { chat = value; OnPropertyChanged("chat"); }
        }
        public void InitTCPServer()
        {
            try
            {
                for (int i = 0; i < INFO.checkListOnline.Length; i++)
                {
                    INFO.checkListOnline[i] = true;
                }
                for (int i = 0; i < INFO.OnlineList.Length; i++)
                {
                    INFO.OnlineList[i] = "";
                }
                //INFO.ADDinCHAT("hello2");
                TCP_SERVER.server = new ServerObject();
                TCP_SERVER.listenThread = new Thread(new ThreadStart(TCP_SERVER.server.Listen));
                TCP_SERVER.listenThread.Start(); //старт потока
                //INFO.ADDinCHAT("hello3");
            }
            catch (Exception ex)
            {
                TCP_SERVER.server.Disconnect();
                INFO.ADDinCHAT(ex.Message);
            }

        }
        public modelview()
        {

            Task.Factory.StartNew(() =>
            {
                InitTCPServer();
                while (true)
                {
                    if (Chat.Length > 20000)
                    {
                        DateTime now = DateTime.Now;
                        INFO.History = "чат был очищен в" + now.Day + "\n";
                    }
                    Chat = INFO.CHAT;
                    Task.Delay(1000).Wait();
                    
                }

            });
        }

    }
}
