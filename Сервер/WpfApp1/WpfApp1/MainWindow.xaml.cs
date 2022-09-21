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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfApp1
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        int ls = 0;
        string id;
        string name;
        public MainWindow()
        {
            InitializeComponent();
        }
        private void titleBar_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            TCP_SERVER.server.Disconnect();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            conditions();
        }

        private void input_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                conditions();
            }    
        }
        private void conditions() // ветвления для консоли
        {
            
            string lowtext = input.Text.ToLower();
            if (lowtext == "/online")
            {
                
                INFO.CHAT += INFO.CompileOnlineList();
                input.Text = "";
            }
            else if (lowtext.StartsWith("/msg"))
            {
                TCP_SERVER.server.ForAllMessage("<Server> " + input.Text.Remove(0,4));
                INFO.ADDinCHAT("<Server> " + input.Text.Remove(0, 4) + "\n");
                input.Text = "";
            }
            else if (lowtext.StartsWith("/kick"))
            {
                TCP_SERVER.server.kick(lowtext.Remove(0,6));
                input.Text = "";
            }

            else if (lowtext.StartsWith("/m"))
            {
                try
                {
                    
                    name = lowtext.Remove(0, 3);
                    id = INFO.FindID(name);
                    INFO.ADDinCHAT("вы переписываетесь с: " + name);
                    ls = 1;
                }
                catch {
                    INFO.ADDinCHAT("не найдено");
                }
                input.Text = "";
            }
            else if (ls == 1)
            {
                if (lowtext.StartsWith("/close"))
                {
                    INFO.ADDinCHAT("вы больше не переписываетесь в личных сообщениях");
                    ls = 0;
                }
                else
                {
                    TCP_SERVER.server.For1Message("/m " + "<server>" + ": " + input.Text,id);
                    INFO.ADDinCHAT("/m " + name + ": " + input.Text);
                }
                
            }
            else if (lowtext.StartsWith("/help"))
            {
                INFO.ADDinCHAT("");
            }
            else
            {
                INFO.ADDinCHAT("Чтото пошло не так...");
                input.Text = "";
            }    
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            TCP_SERVER.server.Disconnect();
            Application.Current.Shutdown();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }
    }
}
