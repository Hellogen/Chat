using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.IO;
namespace WpfApp4
{
    /// <summary>
    /// Логика взаимодействия для auth.xaml
    /// </summary>
    public partial class auth : Window
    {
        InfoCom infoCom = new InfoCom();
        commands.ForCode forCode = new commands.ForCode();
        public auth()
        {
            
            
            
            InitializeComponent();
            try
            { //Предзагрузка данных
            string Preload = forCode.open("options.opt");
            string[] masPreload = forCode.split(Preload);
                IpInput.Text = masPreload[1];
                PortInput.Text = masPreload[2];
                NameInput.Text = masPreload[3];
                if (masPreload[4].Contains("on")) 
                {
                    InfoBank.allowlog = true;
                    if (Directory.Exists("logging") == false)
                        Directory.CreateDirectory("logging");
                }
            }
            catch 
            {
                // string options = "there will be options";
                // forCode.Save(options, "options.opt");
                infoCom.logging("error with reading options in auth");
            }
                
        }
        private void titleBar_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }
       

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            //----------------
            TCP_SOCKET.host = IpInput.Text;
            TCP_SOCKET.port = int.Parse(PortInput.Text);
            TCP_SOCKET.userName = NameInput.Text;
            //----------------

            
            forCode.save_in_opt("options", 0);
            forCode.save_in_opt(TCP_SOCKET.host, 1);
            forCode.save_in_opt(TCP_SOCKET.port.ToString(), 2);

            infoCom.logging( "ip - " + TCP_SOCKET.host + ":" + TCP_SOCKET.port);
            
            forCode.save_in_opt(TCP_SOCKET.userName, 3);
            infoCom.logging("name - " + NameInput.Text);
            TCP_SOCKET.IsWorking = true;
            
            MainWindow main = new MainWindow();
            main.Show();
            Close();
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            try
            {
                System.Diagnostics.Process.Start("Руководство Пользователя.docx");
            }
            catch (Exception ex)
            {
                infoCom.logging("error with open document: " + ex.Message);
            }
        }
    }
}
