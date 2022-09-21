using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp4
{
    static class InfoBank
    {
       public static short exit = 0;
        public static string path_to_file = ""; 
        public static string log = ""; // желательно изменить метод логирования
        public static string path_to_log = "logging\\log" + DateTime.Now.Day + "-" + DateTime.Now.Month + "-" + DateTime.Now.Year + "_" + DateTime.Now.Hour + "." + DateTime.Now.Minute + ".txt";
        public static string error_message = "";
        public static bool allowlog = false;
        public static string ListOnline="Error"; //онлайн лист пользователей 
    }
    public class InfoCom
    {
        commands.ForCode ForCode = new commands.ForCode();
        public void logging(string line_text)
        {
            if (InfoBank.allowlog == true)
            {
                InfoBank.log += DateTime.Now.Hour + ":" + DateTime.Now.Minute + " " + line_text + "\n";
                try
                {
                    ForCode.Save(InfoBank.log, InfoBank.path_to_log);
                }
                catch
                {
                    TCP_SOCKET.CHAT += "Error with log" + "\n";
                }
            }
        }
    }
}
