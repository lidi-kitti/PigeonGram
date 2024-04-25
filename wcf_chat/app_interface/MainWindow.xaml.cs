using login_registration;
using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
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
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;
using app_interface.ServiceReference1;
using System.ServiceModel;
using System.Windows.Interop;

namespace app_interface
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    /// 

    public partial class MainWindow : Window, IServiceChatCallback
    {

        ServiceReference1.ServiceChatClient client;

        public void MsgCallback(string msg)
        {
            //TextBlock receiverMsg = new TextBlock();
            //receiverMsg.Text = msg;
            //receiverMsg.HorizontalAlignment = HorizontalAlignment.Left;
            //chatPanel.Children.Add(receiverMsg);
            messageTest.Items.Add(msg);
        }

        DataBase database = new DataBase();
        public MainWindow()
        {
            InitializeComponent();

            client = new ServiceChatClient(new InstanceContext(this));
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

        private void Button_Back_Reg_Click(object sender, RoutedEventArgs e)
        {
            logWinGrid.Visibility = Visibility.Hidden;
            regWinGrid.Visibility = Visibility.Visible;
        }

        private void Button_Back_Log_Click(object sender, RoutedEventArgs e)
        {
            regWinGrid.Visibility = Visibility.Hidden;
            logWinGrid.Visibility = Visibility.Visible;
        }

        // авторизация
        private void Button_LogIn_Click(object sender, RoutedEventArgs e)
        {
            string login = emailLogBox.Text.Trim().ToLower();
            string password = passwordLogBox.Password.Trim();
            string hashedPass = MyHash.HashPassword(password);

            SqlDataAdapter adapter = new SqlDataAdapter();
            DataTable table = new DataTable();

            string querystring = $"select * from Users where Login = '{login}' and Hashed_Password = '{hashedPass}'";

            SqlCommand command = new SqlCommand(querystring, database.getConnection());

            adapter.SelectCommand = command;
            adapter.Fill(table);

            if (table.Rows.Count == 1)
            {
                MessageBox.Show("Вы успешно вошли!");
                logWinGrid.Visibility = Visibility.Hidden;
                mainWinBorder.Visibility = Visibility.Visible;
                mainWinGrid.Visibility = Visibility.Visible;

                string connectionString = "Data Source=LAPTOP-S3L918JB\\SQLDEGREE;Initial Catalog=Database;Integrated Security=True";
                string query = $"SELECT Login FROM Users";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    using (SqlCommand command2 = new SqlCommand(query, connection))
                    {
                        using (SqlDataReader reader = command2.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                string value = reader["Login"].ToString();
                                usersListBox.Items.Add(value);
                            }
                        }
                    }
                }

            }
            else
            {
                MessageBox.Show("Такого аккаунта не существует.");
            }
            client = new ServiceChatClient(new InstanceContext(this));
        }

        // регистрация
        private void Button_Registration_Click(object sender, RoutedEventArgs e)
        {
            DataBase dataBase = new DataBase();

            string firstName = fisrtNameRegBox.Text.Trim();
            string lastName = lastNameRegBox.Text.Trim();
            string email = emailRegBox.Text.Trim().ToLower();
            string login = logRegBox.Text.Trim();
            string password = passwordRegBox.Password.Trim();
            string confirmPassword = passwordRegBox.Password.Trim();

            if (firstName.Length < 1)
            {
                fisrtNameRegBox.ToolTip = "Это поле введено некорректно!";
                fisrtNameRegBox.Background = Brushes.DarkRed;
            }
            else if (lastName.Length < 1)
            {
                lastNameRegBox.ToolTip = "Это поле введено некорректно!";
                lastNameRegBox.Background = Brushes.DarkRed;
            }
            else if (email.Length < 5 || !email.Contains("@") || !email.Contains("."))
            {
                emailRegBox.ToolTip = "Это поле введено некорректно!";
                emailRegBox.Background = Brushes.DarkRed;
            }
            else if (password.Length < 5)
            {
                MessageBox.Show("Слишком короткий пароль!");
                passwordRegBox.Password = "Это поле введено некорректно!";
                passwordRegBox.Background = Brushes.DarkRed;
            }
            else if (confirmPassword != password)
            {
                passwordRegBox.Password = "Пароли не совпадают!";
                confirmPasswordRegBox.Background = Brushes.DarkRed;
            }
            else
            {
                fisrtNameRegBox.ToolTip = "";
                fisrtNameRegBox.Background = Brushes.Transparent;

                lastNameRegBox.ToolTip = "";
                lastNameRegBox.Background = Brushes.Transparent;

                emailRegBox.ToolTip = "";
                emailRegBox.Background = Brushes.Transparent;

                passwordRegBox.ToolTip = "";
                passwordRegBox.Background = Brushes.Transparent;

                confirmPasswordRegBox.ToolTip = "";
                confirmPasswordRegBox.Background = Brushes.Transparent;

                string hashedPass;
                hashedPass = MyHash.HashPassword(password);

                string querystring = $"insert into Users (First_Name, Last_Name, Email, Login, Hashed_Password, User_Rank, Is_Logged) values ('{firstName}', '{lastName}', '{email}', '{login}', '{hashedPass}', '2', '0')";
                SqlCommand command = new SqlCommand(querystring, dataBase.getConnection());

                dataBase.openConnection();

                if (command.ExecuteNonQuery() == 1)
                {
                   MessageBox.Show("Регистрация успешна. Пожалуйста, войдите в аккаунт для продолжения работы.");
                   regWinGrid.Visibility = Visibility.Hidden;
                   logWinGrid.Visibility = Visibility.Visible;
                }

                else
                {
                    MessageBox.Show("Аккаунт не создан");
                }

                dataBase.closeConnection();
            }
        }

        private void backToMainWinBtn_Click(object sender, RoutedEventArgs e)
        {
            addNewContactGrid.Visibility = Visibility.Hidden;
            mainWinGrid.Visibility = Visibility.Visible;
            mainWinBorder.Visibility = Visibility.Visible;
        }

        private void usersListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            string selectedChat = usersListBox.SelectedItem.ToString();
            beautyPanel.Visibility = Visibility.Hidden;
            //chatPanel.Visibility = Visibility.Visible;
            //chatPanel.Children.Clear();
            messageTest.Visibility = Visibility.Visible;
            
            MessageTB.Visibility = Visibility.Visible;
            sendMsgBtn.Visibility = Visibility.Visible;
            //сюда в будущем добавится метод, позволяющий подгружать сообщения из бд при выборе 
            //элемента listbox

        }

        private void sendMsgBtn_Click(object sender, RoutedEventArgs e)
        {
            if (usersListBox.SelectedItem != null && usersListBox.SelectedItems.Count>0)
            {
                string reciver = usersListBox.SelectedItem.ToString();
                string login = emailLogBox.Text;
                foreach (var selectedUser in usersListBox.SelectedItems)
                {
                    string selectedChat = selectedUser.ToString();
                    string message = MessageTB.Text;
                    client.SendMsg(login, selectedChat, message);
                    MessageTB.Text = string.Empty;
                }
            }
        }
    }
}
