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
using System.Data.SqlClient;
using System.Data;

namespace login_registration
{
    /// <summary>
    /// Логика взаимодействия для LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        DataBase database = new DataBase();

        public LoginWindow()
        {
            InitializeComponent();
        }

        private void Button_Log_Click(object sender, RoutedEventArgs e)
        {
            string email = emailBox.Text.Trim().ToLower();
            string password = passwordBox.Text.Trim();

            SqlDataAdapter adapter = new SqlDataAdapter();
            DataTable table = new DataTable();

            string querystring = $"select * from register where email = '{email}' and password_user = '{password}'";

            SqlCommand command = new SqlCommand(querystring, database.getConnection());

            adapter.SelectCommand = command;
            adapter.Fill(table);

            if(table.Rows.Count == 1) 
            {
                MessageBox.Show("Вы успешно вошли!");
            }
            else
            {
                MessageBox.Show("Такого аккаунта не существует.");
            }
        }
    }
}
