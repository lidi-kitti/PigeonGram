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

namespace app_interface
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

        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                DragMove();
        }

        private void btnMinimize_Click_1(object sender, RoutedEventArgs e)
        //to minimize the window
        {
            WindowState = WindowState.Minimized;
        }

        private void btnMaximize_Click_1(object sender, RoutedEventArgs e)
        //to maximize the window
        {
            if (WindowState == WindowState.Normal)
                WindowState = WindowState.Maximized;
            else
                WindowState = WindowState.Normal;
        }

        private void btnClose_Click_1(object sender, RoutedEventArgs e)
        //to close the window
        {
            Application.Current.Shutdown();
        }

        private void Conversation_Loaded(object sender, RoutedEventArgs e)
        {

        }
    }
}
