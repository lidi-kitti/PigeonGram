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

namespace app_interface
{
    /// <summary>
    /// Логика взаимодействия для LoginRegWindow.xaml
    /// </summary>
    public partial class LoginRegWindow : Window
    {
        public LoginRegWindow()
        {
            InitializeComponent();
        }

        private void logIn_btn_choise_Click(object sender, RoutedEventArgs e)
        {
            logIn_grid.Visibility = Visibility.Visible;
            reg_grid.Visibility = Visibility.Hidden;
        }

        private void reg_btn_choise_Click(object sender, RoutedEventArgs e)
        {
            reg_grid.Visibility = Visibility.Visible;
            logIn_grid.Visibility = Visibility.Hidden;
        }
    }
}
