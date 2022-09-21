using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1
{
   public class commands
    {
       public class ForForms
        {
            /// <summary>
            /// Здесь находятся комманды как save, open, split -ну и как понятно из названий сохранение открытие
            /// </summary>

            internal string[] split(string theText)
            {
                string[] lines = theText.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);
                return lines;
            }
            public void Save(string theText, bool toggle = false)
            {
                SaveFileDialog Saving = new SaveFileDialog();
                string path = null;
               // Saving.DefaultExt = ".txt";
              //  Saving.Filter = "Text documents (.txt)|*.txt";
                if (Saving.ShowDialog() == true)
                {
                    if (Saving.FileName != "")
                    {
                        path = Saving.FileName;
                        using (FileStream file1 = new FileStream(path, FileMode.Create, FileAccess.ReadWrite))
                        {
                            StreamWriter writter = new StreamWriter(file1);
                            writter.WriteLine(theText);
                            writter.Close();
                            if (toggle == true)
                            {
                                Process.Start(path);
                            }
                        }
                    }

                }

            }
            //
            public void Save_with_path_to_file(string theText, string path_to_file, bool toggle = false)
            {
                SaveFileDialog Saving = new SaveFileDialog();
                string path = "";
                if (Saving.ShowDialog() == true)
                {
                    if (Saving.FileName != "")
                    {
                        path = Saving.FileName;
                        path_to_file = Saving.FileName;
                        using (FileStream file1 = new FileStream(path, FileMode.Create, FileAccess.ReadWrite))
                        {
                            StreamWriter writter = new StreamWriter(file1);
                            writter.WriteLine(theText);
                            writter.Close();
                            if (toggle == true)
                            {
                                
                            }
                        }
                    }
                }
                
            }
            
            //
            public string open()
            {
                string theText;
                string path = null;
                OpenFileDialog open = new OpenFileDialog();
                try
                {

                    if (open.ShowDialog() == true)
                    {
                        path = open.FileName;
                    }
                }
                catch
                {

                }
       
                try//------------------------------------------------------------------------------
                {
                    using (FileStream op = File.OpenRead(path))
                    {
                        byte[] array = new byte[op.Length];
                        op.Read(array, 0, array.Length);
                        theText = System.Text.Encoding.Default.GetString(array);
                    }
                    return theText;
                }
                // код ...
                catch
                {
                    return "error";
                    // обработка ошибки

                }
            }
            public string open_with_path(string path_to_file)
            {
                string theText;
                string path = null;
                OpenFileDialog open = new OpenFileDialog();
                try
                {

                    if (open.ShowDialog() == true)
                    {
                        path = open.FileName;
                        path_to_file = open.FileName ;
                    }
                }
                catch
                {

                }

                try//------------------------------------------------------------------------------
                {
                    using (FileStream op = File.OpenRead(path))
                    {
                        byte[] array = new byte[op.Length];
                        op.Read(array, 0, array.Length);
                        theText = System.Text.Encoding.Default.GetString(array);
                    }
                    
                    return theText;
                    
                }
                // код ...
                catch
                {
                    return "error";
                    // обработка ошибки

                }
            }





        }
      public class ForCode
        {
            internal string[] splitREE(string theText) // разделить текст для кода
            {
                string[] lines = theText.Split(new[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
                //, "\r", "\n"
                return lines;
            }
            internal string[] split(string theText) // разделить текст для кода
            {
                string[] lines = theText.Split(new[] { "\r\n" }, StringSplitOptions.None);
                //, "\r", "\n"
                return lines;
            }
            internal List<string> split_to_list(string theText)
            {
                string[] lines = theText.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries);
                List<string> list = new List<string>();
                for (int i = 0; i < lines.Length; i++)
                {
                    list.Add(lines[i]);
                }
                return list;
            }
            internal string unit(string[] theText) // соеденить текст для кода
            {
                
                string EndText = " ";
                
                for (int i = 0; i <+ theText.Length; i++)
                {
                    if (i != theText.Length-1)
                    {
                        EndText += theText[i] + "\r\n";
                    }
                    else
                    {
                        EndText += theText[i];
                        
                    }
                }
                return EndText;
            }
            public string unitk(string[] thetext, int k)
            {
                string result = "";
                for ( int i = 0; i < k; i++)
                {
                    if (i == k-1)
                    {
                        result += thetext[i];
                    }
                    else
                    {
                        result += thetext[i] + "\r\n";
                    }
                }
                return result;
            }
          
            public string open(string path, bool toggle = false)
            {
                string theText;

                try//------------------------------------------------------------------------------
                {
                   
                    using (FileStream op = File.OpenRead(path))
                    {
                        
                        byte[] array = new byte[op.Length];
                        op.Read(array, 0, array.Length);
                        theText = System.Text.Encoding.UTF8.GetString(array);
                    }
                    return theText;
                }
                // код ...
                catch
                {
                    if (toggle == true)
                    {
                        return "error";
                    }
                    else
                    {
                        return "";
                    }
                    // обработка ошибки

                }
            }
          

            public void Save(string theText, string path ,bool toggle = false)
            {
                SaveFileDialog Saving = new SaveFileDialog();
                
                // Saving.DefaultExt = ".txt";
                //  Saving.Filter = "Text documents (.txt)|*.txt";
               
                        using (FileStream file1 = new FileStream(path, FileMode.Create, FileAccess.ReadWrite))
                        {
                            StreamWriter writter = new StreamWriter(file1);
                            writter.WriteLine(theText);
                            writter.Close();
                            if (toggle == true)
                            {
                                Process.Start(path);
                            }
                        }
                    

                

            }
            public void save_in_opt(string TheText, int index) // Сохранение настроек
            {
                
                try
                {
                    const int k = 6; //- количество строк
                    string[] lines = new string[k] { "0", "1", "2", "3", "4","fine" };
                    string[] redactinglines = new string[k] { "0", "1", "2", "3", "4", "notfine" };
                    int outing = 0;
                    /// 0 - -options!-
                    /// 1 - ip
                    /// 2 - port
                    /// 3 - name
                    /// 4 - on/off logs
                    ///
                    if (File.Exists("options.opt"))
                    {
                        
                        string text = open("options.opt");
                        lines = split(text);
                        for (int i = 0; i < k; i++)
                        {   
                            try
                            {
                                redactinglines[i] = lines[i];
                            }
                            catch
                            {
                                //infoCom.logging(i + "was removed");
                            }
                            
                        }
                        if (redactinglines[5] != "fine")
                        {
                            redactinglines[0] = "-options-";
                            redactinglines[1] = "";
                            redactinglines[2] = "";
                            redactinglines[3] = "";
                            redactinglines[4] = "off";
                            redactinglines[5] = "fine";
                        }
                        else
                        {
                           // infoCom.logging("allthefine");
                        }
                    }
                    else
                    {

                        redactinglines[0] = "-options-";
                        redactinglines[1] = "";
                        redactinglines[2] = "";
                        redactinglines[3] = "";
                        redactinglines[4] = "off";
                        redactinglines[5] = "fine";

                    }

                   // infoCom.logging("all fine");

                    

                    if (index < k && outing == 0)
                    {
                        
                        redactinglines[index] = TheText;
                        string textik = unit(redactinglines);
                        Save(textik, "options.opt");
                    }
                    
                }
                catch (Exception ex)
                {
                    
                    //infoCom.logging("error with writting or reading options: " + ex.Message );
                }
                
                
                
            }
            public void save_in_personalization(string TheText, int index)
            {

            }







        }
        
    }
}
