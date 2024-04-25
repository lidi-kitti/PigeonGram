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
using Microsoft.Win32;
using System.IO;

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

                string path = "C:\\Users\\samsung\\Pictures\\user with no photo.png";
                byte[] imageBytes = File.ReadAllBytes(path);

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

                string connectionString = "Data Source=LAPTOP-S3L918JB\\SQLDEGREE;Initial Catalog=Database;Integrated Security=True";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string querystring = $"INSERT INTO Users (First_Name, Last_Name, Email, Login, Hashed_Password, User_Rank, Is_Logged, User_Photo) " +
                                         $"VALUES ('{firstName}', '{lastName}', '{email}', '{login}', '{hashedPass}', '2', '0', @UserPhoto)";

                    using (SqlCommand command = new SqlCommand(querystring, connection))
                    {
                        command.Parameters.AddWithValue("@UserPhoto", imageBytes);

                        int rowsAffected = command.ExecuteNonQuery();

                        if (rowsAffected == 1)
                        {
                            MessageBox.Show("Регистрация успешна. Пожалуйста, войдите в аккаунт для продолжения работы.");
                            logWinGrid.Visibility = Visibility.Visible;
                            regWinGrid.Visibility = Visibility.Hidden;
                        }
                        else
                        {
                            MessageBox.Show("Аккаунт не создан");
                        }
                    }
                }
            }
           
        }
           

        private void backToMainWinBtn_Click(object sender, RoutedEventArgs e)
        {
            myRrofileInfoGrid.Visibility = Visibility.Hidden;
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

        private void myProfileInfo_Click(object sender, RoutedEventArgs e)
        {
            myRrofileInfoGrid.Visibility = Visibility.Visible;
            mainWinGrid.Visibility = Visibility.Hidden;
            mainWinBorder.Visibility = Visibility.Hidden;
            string login = emailLogBox.Text;
            string connectionString = "Data Source=LAPTOP-S3L918JB\\SQLDEGREE;Initial Catalog=Database;Integrated Security=True";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string querystring = $"SELECT User_Photo FROM Users where Login = '{login}'";

                using (SqlCommand command = new SqlCommand(querystring, connection))
                {
                    command.Parameters.AddWithValue("@Login", login);

                    byte[] imageBytes = (byte[])command.ExecuteScalar();

                    if (imageBytes != null)
                    {
                        BitmapImage bitmapImage = new BitmapImage();
                        bitmapImage.BeginInit();
                        bitmapImage.StreamSource = new MemoryStream(imageBytes);
                        bitmapImage.EndInit();

                        pictureSet.Source = bitmapImage;
                    }
                    else
                    {
                        MessageBox.Show("Изображение не найдено.");
                    }
                }
            }

        }

        string picAddress = "";
        private void newPhoto_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFile = new OpenFileDialog();
            if ((bool)openFile.ShowDialog())
            {
                picAddress = openFile.FileName;
                pictureSet.Source =
                    new BitmapImage(new Uri(openFile.FileName, UriKind.Absolute)) { CreateOptions = BitmapCreateOptions.IgnoreImageCache }; ;
            }
        }

        private void changeProfileBtn_Click(object sender, RoutedEventArgs e)
        {
            string login = emailLogBox.Text;
            BitmapImage bitmapImage = (BitmapImage)pictureSet.Source;

            byte[] imageBytes;
            using (MemoryStream memoryStream = new MemoryStream())
            {
                JpegBitmapEncoder encoder = new JpegBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(bitmapImage));
                encoder.Save(memoryStream);
                imageBytes = memoryStream.ToArray();
            }

            string connectionString = "Data Source=LAPTOP-S3L918JB\\SQLDEGREE;Initial Catalog=Database;Integrated Security=True";
            string query = "UPDATE Users SET User_Photo = @User_Photo WHERE Login = @Login";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.Add("@User_Photo", SqlDbType.VarBinary, -1).Value = imageBytes;
                    command.Parameters.Add("@Login", SqlDbType.VarChar).Value = login;

                    try
                    {
                        connection.Open();
                        command.ExecuteNonQuery();
                        MessageBox.Show("Изображение успешно обновлено в базе данных.");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Произошла ошибка при обновлении изображения в базе данных: " + ex.Message);
                    }
                }
            }
        }
    }
}
