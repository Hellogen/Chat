using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1
{
    internal class cats_smiles
    {
        public string catChoice(string indexofcat, string name)
        {
            string cat = "";
            string StartOrEnd = "#############\n";
            if (indexofcat == "/cats 1")
            {
                cat += StartOrEnd;
                cat += "|                    ／＞　 フ" + "\n";
                cat += "|　　　　　| 　_　 _|" + "\n";
                cat += "|　 　　　／`ミ _x 彡" + "\n";
                cat += "|　　 　 /　　　 　 |" + "\n";
                cat += "|　　　 /　 ヽ　　 ﾉ" + "\n";
                cat += "|　／￣|　　 | | |" + "\n";
                cat += "|　| (￣ヽ＿_ヽ_)_)" + "\n";
                cat += "| 　＼二つ " + "\n";
                cat += StartOrEnd;
                return name + ": " + "\n" + cat;
            }
            return "Ничего не найдено";
        }
        public string catHelp()
        {
            return "";
        }
    }
}
