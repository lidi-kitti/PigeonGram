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
using System.Data.SqlClient;
using app_interface;
//using System.Data.Entity;

namespace login_registration
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class RegWindow : Window
    {
        public RegWindow()
        {
            InitializeComponent();
        }

        private void Button_Reg_Click(object sender, RoutedEventArgs e)
        {

            DataBase dataBase = new DataBase();

            string firstName = fisrtNameBox.Text.Trim();
            string lastName = lastNameBox.Text.Trim();
            string email = emailBox.Text.Trim().ToLower();
            string password = passwordBox.Password.Trim();
            string confirmPassword = confirmPasswordBox.Password.Trim();

            if(firstName.Length<1)
            {
                fisrtNameBox.ToolTip = "Это поле введено некорректно!";
                fisrtNameBox.Background = Brushes.DarkRed;
            } 
            else if(lastName.Length<1)
            {
                lastNameBox.ToolTip = "Это поле введено некорректно!";
                lastNameBox.Background = Brushes.DarkRed;
            }
            else if(email.Length<5 || !email.Contains("@") || !email.Contains("."))
            {
                emailBox.ToolTip = "Это поле введено некорректно!";
                emailBox.Background = Brushes.DarkRed;
            }
            else if(password.Length<5)
            {
                passwordBox.ToolTip = "Это поле введено некорректно!";
                passwordBox.Background = Brushes.DarkRed;
            }
            else if(confirmPassword != password)
            {
                confirmPasswordBox.ToolTip = "Пароли не совпадают!";
                confirmPasswordBox.Background = Brushes.DarkRed;
            }
            else
            {
                fisrtNameBox.ToolTip = "";
                fisrtNameBox.Background = Brushes.Transparent;

                lastNameBox.ToolTip = "";
                lastNameBox.Background = Brushes.Transparent;

                emailBox.ToolTip = "";
                emailBox.Background = Brushes.Transparent;

                passwordBox.ToolTip = "";
                passwordBox.Background = Brushes.Transparent;

                confirmPasswordBox.ToolTip = "";
                confirmPasswordBox.Background = Brushes.Transparent;

                string querystring = $"insert into users_db (first_name, last_name, email, password_user) values ('{firstName}', '{lastName}', '{email}', '{password}')";
                SqlCommand command = new SqlCommand(querystring, dataBase.getConnection());

                dataBase.openConnection();

                if (command.ExecuteNonQuery() == 1)
                {
                    MessageBox.Show("Регистрация успешна");

                    MainWindow mainWindow = new MainWindow();
                    mainWindow.Show();

                    this.Hide();

                }

                else
                {
                    MessageBox.Show("Аккаунт не создан");
                }

                dataBase.closeConnection();
            }           
        }

        private void Button_Log_Win_Click(object sender, RoutedEventArgs e)
        {
            LoginWindow loginWindow = new LoginWindow();
            loginWindow.Show();
            this.Hide();
        }
    }
}
