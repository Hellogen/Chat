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

namespace WpfApp4
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {

            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (sendMES.Text == "")
            { }
            else
            {
                TCP_SOCKET.SendMessage(sendMES.Text);
                sendMES.Text = "";
                if (sendMES.Text == "/Exit")
                {
                    InfoCom infoCom = new InfoCom();
                    TCP_SOCKET.DisconnectEx();
                    Application.Current.Shutdown();
                }
            }
        }

        private void titleBar_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void Close(object sender, RoutedEventArgs e)
        {
                TCP_SOCKET.SendMessage("/Exit");
                TCP_SOCKET.Disconnect();
                Application.Current.Shutdown();
        }

        private void TextBlock_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            Scroll.ScrollToEnd();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            TCP_SOCKET.SendMessage("/Exit");
            TCP_SOCKET.Disconnect();
            Application.Current.Shutdown();
        }

        private void sendMES_KeyDown(object sender, KeyEventArgs e)
        {
            if (sendMES.Text == "")
            { }
            else
            {
                if (e.Key == Key.Enter)
                {
                    TCP_SOCKET.SendMessage(sendMES.Text);
                    sendMES.Text = "";
                    if (sendMES.Text == "/Exit")
                    {
                        TCP_SOCKET.SendMessage("/Exit");
                        InfoCom infoCom = new InfoCom();
                        TCP_SOCKET.DisconnectEx();
                        Application.Current.Shutdown();
                    }
                }
            }
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            /*
                　／＞　 フ
        　　　　　| 　_　 _|
        　 　　　／`ミ _x 彡
        　　 　 /　　　 　 |
        　　　 /　 ヽ　　 ﾉ
        　／￣|　　 |　|　|
        　| (￣ヽ＿_ヽ_)_)
         　＼二つ 
             */
        }
    }
}
