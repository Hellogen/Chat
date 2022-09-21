using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1
{
    internal class crypt
    {
        string alp = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ !,./<>-+=_\"\'\n:()абвгдеёжзийклмнопрстуфхцчшщъыьэюяАБВГДЕЁЖЗИЙКЛМНОПРСТУФХЦЧШЩЪЫЬЭЮЯ1234567890";
        int step = 5;
        public string Encrypt(string text)
        {
            string encryptedstr = "";
            int index = 0;
            for (int i = 0; i < text.Length; i++)
            {
                if (alp.IndexOf(text.Substring(i, 1)) == -1)
                {
                    encryptedstr += text.Substring(i, 1);
                }
                else
                {
                    index = alp.IndexOf(text.Substring(i, 1)) + step;
                    if (index >= alp.Length) index -= alp.Length;
                    encryptedstr += alp.Substring(index, 1);
                }    
                
            }
            return encryptedstr;
        }
        public string UnEncrypt(string text)
        {
            int index = 0;
            string UnEncryptedText = "";
            for (int i = 0; i < text.Length; i++)
            {
                if (alp.IndexOf(text.Substring(i, 1)) == -1)
                {
                    UnEncryptedText += text.Substring(i, 1);
                }
                else
                {
                    index = alp.IndexOf(text.Substring(i, 1));
                    if (index < step)
                    {
                        index = alp.Length + index - step;
                    }
                    else
                    {
                        index -= step;
                    }
                    if (index >= alp.Length) index -= alp.Length;
                    UnEncryptedText += alp.Substring(index, 1);
                }
                
            }
            return UnEncryptedText;
        }
    }
}
