using app_interface.Commands;
using app_interface.CustomControls;
using app_interface.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace app_interface.ViewModels
{
    public class ViewModel: INotifyPropertyChanged
    {
        #region MainWindow
        #region Properties
        public string ContactName { get; set; }
        public Uri ContactPhoto { get; set; }
        public string LastSeen { get; set; }
        #endregion
        #endregion
        #region Status Thumbs
        #region Properties
        #region Logics
        public ObservableCollection<StatusDataModel> statusThumbsCollection { get; set; }
        #endregion
        
        void LoadStatusThumbs()
        {
            statusThumbsCollection = new ObservableCollection<StatusDataModel>()
            {
                new StatusDataModel{
                   IsMeAddStatus = true
                },
                new StatusDataModel{
                    ContactName="Майк",
                    ContactPhoto=new Uri("/Assets/1.png",UriKind.RelativeOrAbsolute),
                    StatusImage=new Uri("/Assets/5.jpg",UriKind.RelativeOrAbsolute),
                    IsMeAddStatus=false
                },
                new StatusDataModel{
                    ContactName="Стив",
                    ContactPhoto=new Uri("/Assets/2.jpg",UriKind.RelativeOrAbsolute),
                    StatusImage=new Uri("/Assets/8.jpg",UriKind.RelativeOrAbsolute),
                    IsMeAddStatus=false

                },
                new StatusDataModel{
                    ContactName="Вилл",
                    ContactPhoto=new Uri("/Assets/3.png",UriKind.RelativeOrAbsolute),
                    StatusImage=new Uri("/Assets/5.jpg",UriKind.RelativeOrAbsolute),
                    IsMeAddStatus=false

                },
                  new StatusDataModel{
                    ContactName="Джон",
                    ContactPhoto=new Uri("/Assets/4.png",UriKind.RelativeOrAbsolute),
                    StatusImage=new Uri("/Assets/3.jpg",UriKind.RelativeOrAbsolute),
                    IsMeAddStatus=false

                },

            };
            OnProperyChanged("StatusThumbsCollection");
        }
        #endregion
        #region Chats List
        #region Properties
        public ObservableCollection<ChatListData> Chats { get; set; }
        #endregion

        #region Logics
        void LoadChats()
        {
            Chats = new ObservableCollection<ChatListData>()
            {
                new ChatListData
                {
                    ContactName="Билл",
                    ContactPhoto=new Uri("/Assets/6.jpg",UriKind.RelativeOrAbsolute),
                    Message="Привет, как дела?",
                    LastMessageTime="вт, 15:28",
                    ChatIsSelected=true
                },
                new ChatListData
                {
                    ContactName="Майк",
                    ContactPhoto=new Uri("/Assets/1.png",UriKind.RelativeOrAbsolute),
                    Message="Проверь почту",
                    LastMessageTime="пн, 12:01"
                },
                new ChatListData
                {
                    ContactName="Стив",
                    ContactPhoto=new Uri("/Assets/7.png",UriKind.RelativeOrAbsolute),
                    Message="Да, было весело)",
                    LastMessageTime="вт, 08:10"
                },
                new ChatListData
                {
                    ContactName="Джон",
                    ContactPhoto=new Uri("/Assets/8.jpg",UriKind.RelativeOrAbsolute),
                    Message="Как ты?",
                    LastMessageTime="вт, 01:00"
                }
            };
            OnProperyChanged();
        }
        #endregion
        #region Commands
        // У МЕНЯ ВЕРСИЯ ЯЗЫКА НЕ ПОСЛЕДНЯЯ И ЭТА ХУЙНЯ НЕ РАБОТАЕт АААААААААААААААААААААААААААААААААААААААААААААААААААААААААААА
        //protected ICommand _getSelectedChatCommand;
        //public ICommand GetSelectedChatCommand {get
        //    {
        //        ICommand command =  _getSelectedChatCommand ??= new RelayCommand(parameter =>
        //                {
        //                    if (parameter is ChatListData v)
        //                    {
        //                        ContactName = v.ContactName;
        //                        OnProperyChanged("ContactName");
        //                        ContactPhoto = v.ContactPhoto;
        //                        OnProperyChanged("ContactPhoto");
        //                    }
        //                });
        //        return command;
        //    }
        //}
        #endregion

        #endregion
        #region Conversations

        #region Properties
        protected ObservableCollection<ChatConversation> mConversations;
        public ObservableCollection<ChatConversation> Conversations
        {
            get => mConversations;
            set
                {
                mConversations = value;
                OnProperyChanged();
            }
        }

        public ICommand GetSelectedChatCommand => throw new NotImplementedException();
        #endregion
        #region Logics
        void LoadChatConversation()
        {
            if (connection.State == System.Data.ConnectionState.Closed)
                connection.Open();
            if(Conversations == null)
                Conversations = new ObservableCollection<ChatConversation>();
            using (SqlCommand com = new SqlCommand("select * from conversations where ContactName='Майк'", connection))
            {
                using (SqlDataReader reader = com.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string MsgRecievedOn = !string.IsNullOrEmpty(reader["MsgRecievedOn"].ToString()) ?
                            Convert.ToDateTime(reader["MsgRecievedOn"].ToString()).ToString("MMM dd, hh:mm tt") : "";
                        string MsgSentOn = !string.IsNullOrEmpty(reader["MsgSentOn"].ToString()) ?
    Convert.ToDateTime(reader["MsgSentOn"].ToString()).ToString("MMM dd, hh:mm tt") : "";

                        var conversation = new ChatConversation()
                        {
                            ContactName = reader["ContactName"].ToString(),
                            RecievedMessage = reader["RecievedMsgs"].ToString(),
                            MsgRecievedOn = MsgRecievedOn,
                            MsgSentOn = MsgSentOn,
                            SentMessage = reader["SentMsgs"].ToString(),
                            IsMessageRecieved = string.IsNullOrEmpty(reader["RecievedMsgs"].ToString()) ? false: true
                        };
                        Conversations.Add(conversation);
                        OnProperyChanged("Conversations");
                    }
                }
            }
        }
        #endregion

        #endregion
        SqlConnection connection = new SqlConnection(@"Data Source=LAPTOP-N9J4H06Q;Initial Catalog=Database;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");
        public ViewModel()
        {
            LoadStatusThumbs();
            LoadChats();
            LoadChatConversation();
        }
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnProperyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this,new PropertyChangedEventArgs(propertyName));
        }
    }
}
#endregion