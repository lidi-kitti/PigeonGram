using Microsoft.AspNetCore.SignalR.Client;
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

namespace app_interface.CustomControls
{
    /// <summary>
    /// Логика взаимодействия для Conversation.xaml
    /// </summary>
    public partial class Conversation : UserControl
    {
        //HubConnection hubConnection;
        public Conversation()
        {
            InitializeComponent();
   //         hubConnection = new HubConnectionBuilder().WithUrl("https://localhost:7098/chat")
   //.Build();
   //         hubConnection.On<string, string>("ReceivedMessage", (user, message) =>
   //         {
   //             Dispatcher.Invoke(() =>
   //             {
   //                 var newMessage = $"{user}: {message}";
   //                 chatbox.Items.Insert(0, newMessage);
   //             });
   //         });
        }
        // обработчик загрузки окна
        //private async void Window_Loaded(object sender, RoutedEventArgs e)
        //{
        //    try
        //    {
        //        // подключемся к хабу
        //        await hubConnection.StartAsync();
        //        chatbox.Items.Add("Вы вошли в чат");
        //        sendBtn.IsEnabled = true;
        //    }
        //    catch (Exception ex)
        //    {
        //        chatbox.Items.Add(ex.Message);
        //    }
        //}
    }
}
