using ChatServer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace WpfApp1
{
    public static class INFO // класс информации 
    {
        public static int k = 2; // количество человек в чате (ограничение) 
        public static int CountClients = 0;
        public static string History = "history \n";
        public static string CHAT = "";
        public static string Message = "";
        public static string[] OnlineList = new string[k]; //имя
        public static bool[] checkListOnline = new bool[k]; //чек лист
        public static string[] ClientID = new string[k]; // id клиента
        
        public static int InsertOnlineList(string name, string id) //  добавление пользователя в таблицу с его ID.
        {
            int settedindex = 0;
            for (int i = 0; i < OnlineList.Length; i++)
            {
                if (checkListOnline[i] == true)
                {
                    OnlineList[i] = name;
                    checkListOnline[i] = false;
                    ClientID[i] = id;
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
            CountClients = count;
            return result;
        }
        public static string FindID(string name)
        {
            for (int i = 0; i < k; i++)
            {
                if (OnlineList[i].ToLower() == name.ToLower())
                {
                    return ClientID[i];
                }
            }

            return "-1";

        }
        public static void ADDinCHAT(string text)
        {
            INFO.CHAT += text + "\n";
        }
    }

    public static class TCP_SERVER
    {
        public static ServerObject server; // сервер
        public static Thread listenThread; // прослушивание потока


    }
}
