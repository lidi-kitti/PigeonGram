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
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace app_interface.ViewModels
{
    public class ViewModel : INotifyPropertyChanged
    {
        //initializing resource dictionary file
       // private readonly ICommand _command;
        private readonly ResourceDictionary dictionary = 
            Application.LoadComponent(new Uri("app_interface;component/Assets/icons.xaml",
                UriKind.RelativeOrAbsolute)) as ResourceDictionary;
        #region MainWindow
        #region Properties
        public string ContactName { get; set; }
        public byte[] ContactPhoto { get; set; }
        public string LastSeen { get; set; }


        #region Search Chats
        protected bool _isSearchBoxOpen { get; set; }
        public bool IsSearchBoxOpen
        {
            get => _isSearchBoxOpen;
            set
            {
                if (_isSearchBoxOpen == value) return;
                _isSearchBoxOpen = value;
                if (_isSearchBoxOpen != false) SearchText = string.Empty;
                OnPropertyChanged("IsSearchBoxOpen");
                OnPropertyChanged("SearchText");
            }
        }

        protected string LastSearchText { get; set; }
        protected string mSearchText { get; set;}
        public string SearchText
        {
            get => mSearchText;
            set
            {
                //checked if value is different
                if (mSearchText == value) { return; }
                //update view
                mSearchText = value;
                //if search text is empty restore message
                if (string.IsNullOrEmpty(SearchText))
                    Search();
            }
        }
        //this is our list containing the window options...
        private ObservableCollection<MoreOptionsMenu> _windowMoreOptionsMenuList;
        public ObservableCollection<MoreOptionsMenu> WindowMoreOptionsMenuList 
        {
            get {
                return _windowMoreOptionsMenuList;
            }

            set
            {
                _windowMoreOptionsMenuList = value;
            }
        }

        private ObservableCollection<MoreOptionsMenu> _attachmentOptionsMenuList;
        public ObservableCollection<MoreOptionsMenu>AttachmentOptionsMenuList
        {
            get
            {
                return _attachmentOptionsMenuList;
            }

            set
            {
                _attachmentOptionsMenuList = value;
            }
        }
        #endregion
        #endregion

        #region Logics

        #region Window: More Options Popup
        void WindowMoreOptionsMenu()
        {
            WindowMoreOptionsMenuList = new ObservableCollection<MoreOptionsMenu>()
            {
                new MoreOptionsMenu()
                {
                    Icon = (PathGeometry)dictionary["newgroup"],
                    MenuText = "New Group"
                },
                new MoreOptionsMenu()
                {
                    Icon = (PathGeometry)dictionary["newbroadcast"],
                    MenuText = "New Broadcast"
                },
                 new MoreOptionsMenu()
                {
                    Icon = (PathGeometry)dictionary["starredmessages"],
                    MenuText = "Starred Messages"
                },
                new MoreOptionsMenu()
                {
                    Icon = (PathGeometry)dictionary["settings"],
                    MenuText = "Settings"
                },
            };
            OnPropertyChanged("WindowMoreOptionsMenuList");
        }

        void ConversationScreenMoreOptionsMenu()
        {
            //to populate menu items for conversation screen options list..
            WindowMoreOptionsMenuList = new ObservableCollection<MoreOptionsMenu>()
            {
                new MoreOptionsMenu()
                {
                    Icon = (PathGeometry)dictionary["all media"],
                    MenuText = "All Media"
                },
                new MoreOptionsMenu()
                {
                    Icon = (PathGeometry)dictionary["wallpaper"],
                    MenuText = "Change Wallpaper"
                },
                 new MoreOptionsMenu()
                {
                    Icon = (PathGeometry)dictionary["report"],
                    MenuText = "Report"
                },
                new MoreOptionsMenu()
                {
                    Icon = (PathGeometry)dictionary["block"],
                    MenuText = "Block"
                },
                 new MoreOptionsMenu()
                {
                    Icon = (PathGeometry)dictionary["clearchat"],
                    MenuText = "Clear Chat"
                },
                  new MoreOptionsMenu()
                {
                    Icon = (PathGeometry)dictionary["exportchat"],
                    MenuText = "Export Chat"
                },
                   
            };
            OnPropertyChanged("WindowMoreOptionsMenuList");
        }

        void AttachmentOptionsMenu()
        {
            //to populate menu items for attachment menu options list..
            AttachmentOptionsMenuList = new ObservableCollection<MoreOptionsMenu>()
            {
                new MoreOptionsMenu()
                {
                    Icon = (PathGeometry)dictionary["docs"],
                    MenuText = "Docs",
                    BorderStroke="#3F3990",
                    Fill="#CFCEEC"
                },
                new MoreOptionsMenu()
                {
                    Icon = (PathGeometry)dictionary["gallery"],
                    MenuText = "Gallery",
                    BorderStroke="#3F3990",
                    Fill="#CFCEEC"
                },
                 new MoreOptionsMenu()
                {
                    Icon = (PathGeometry)dictionary["audio"],
                    MenuText = "Audio",
                    BorderStroke="#3F3990",
                    Fill="#CFCEEC"
                },
                  new MoreOptionsMenu()
                {
                    Icon = (PathGeometry)dictionary["location"],
                    MenuText = "Location",
                    BorderStroke="#3F3990",
                    Fill="#CFCEEC"
                },
                   new MoreOptionsMenu()
                {
                    Icon = (PathGeometry)dictionary["contact"],
                    MenuText = "Contact",
                    BorderStroke="#3F3990",
                    Fill="#CFCEEC"
                },

            };
            OnPropertyChanged("AttachmentOptionsMenuList");
        }
        #endregion

        public void OpenSearchBox()
        {
            IsSearchBoxOpen = true;
        }
        public void ClearSearchBox()
        {
            if (!string.IsNullOrEmpty(SearchText)) SearchText = string.Empty;
            else CloseSearchBox();
        }
        public void CloseSearchBox() => IsSearchBoxOpen = false;
        
        public void Search()
        {
            //to avoid searching same text again
            if((string.IsNullOrEmpty(LastSearchText) && string.IsNullOrEmpty(SearchText)) || string.Equals(LastSearchText,SearchText))
                    { return; }

            //if searchbox is empty or chat in null pr char cound less then 0
            if (string.IsNullOrEmpty(SearchText) || Chats == null || Chats.Count <= 0)
            {
                FilteredChats = new ObservableCollection<ChatListData>(Chats ?? Enumerable.Empty<ChatListData>());
                OnPropertyChanged("FilteredChats");

                FilteredPinnedChats = new ObservableCollection<ChatListData>(PinnedChats ?? Enumerable.Empty<ChatListData>());
                OnPropertyChanged("FilteredPinnedChats");
                //update last search text
                LastSearchText= SearchText;
                return;
            }

            //now, to find all chats that contain the text in our search box
            FilteredChats = new ObservableCollection<ChatListData>(
                Chats.Where(
                    chat => chat.ContactName.ToLower().Contains(SearchText)
                    || 
                    chat.Message!= null && chat.Message.ToLower().Contains(SearchText)
                    ));
            OnPropertyChanged("FilteredChats");

            FilteredPinnedChats = new ObservableCollection<ChatListData>(
                PinnedChats.Where(
                    pinnedchat => pinnedchat.ContactName.ToLower().Contains(SearchText)
                    ||
                    pinnedchat.Message != null && pinnedchat.Message.ToLower().Contains(SearchText)
                    ));
            OnPropertyChanged("FilteredPinnedChats");
            //update last search text
            LastSearchText = SearchText;
        }
        #endregion

        #region Commands
        protected ICommand _windowsMoreOptionsCommand;
        public ICommand WindowsMoreOptionsCommand
        {
            get
            {
                if (_windowsMoreOptionsCommand == null)
                    _windowsMoreOptionsCommand = new CommandViewModel(WindowMoreOptionsMenu);
                return _windowsMoreOptionsCommand;
            }
            set
            {
                _windowsMoreOptionsCommand = value;
            }
        }
    
        protected ICommand _conversationScreenMoreOptionsCommand;
        public ICommand ConversationScreenMoreOptionsCommand
        {
            get
            {
                if (_conversationScreenMoreOptionsCommand == null)
                    _conversationScreenMoreOptionsCommand = new CommandViewModel(ConversationScreenMoreOptionsMenu);
                return _conversationScreenMoreOptionsCommand;
            }
            set
            {
                _conversationScreenMoreOptionsCommand = value;
            }
        }
        protected ICommand _attachmentOptionsCommand;
        public ICommand AttachmentOptionsCommand
        {
            get
            {
                if (_attachmentOptionsCommand == null)
                    _attachmentOptionsCommand = new CommandViewModel(AttachmentOptionsMenu);
                return _attachmentOptionsCommand;
            }
            set
            {
                _attachmentOptionsCommand = value;
            }
        }
        protected ICommand _openSearchCommand;
        public ICommand OpenSearchCommand
        {
            get
            {
                if (_openSearchCommand == null)
                    _openSearchCommand = new CommandViewModel(OpenSearchBox);
                return _openSearchCommand;
            }
            set
            {
                _openSearchCommand = value;
            }
        }
        protected ICommand _closeSearchCommand;
        public ICommand CloseSearchCommand
        {
            get
            {
                if (_closeSearchCommand == null)
                    _closeSearchCommand = new CommandViewModel(CloseSearchBox);
                return _closeSearchCommand;
            }
            set
            {
                _closeSearchCommand = value;
            }
        }
        protected ICommand _searchCommand;
        public ICommand SearchCommand
        {
            get
            {
                if (_searchCommand == null)
                    _searchCommand = new CommandViewModel(Search);
                return _searchCommand;
            }
            set
            {
                _searchCommand = value;
            }
        }
        protected ICommand _clearSearchCommand;
        public ICommand ClearSearchCommand
        {
            get
            {
                if (_clearSearchCommand == null)
                    _clearSearchCommand = new CommandViewModel(ClearSearchBox);
                return _clearSearchCommand;
            }
            set
            {
                _clearSearchCommand = value;
            }
        }

        #endregion

        #endregion
        #region Status Thumbs

        #region Properties
        #region Logics
        public ObservableCollection<StatusDataModel> StatusThumbsCollection { get; set; }
        #endregion
        
        void LoadStatusThumbs()
        {
            StatusThumbsCollection = new ObservableCollection<StatusDataModel>()
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
            OnPropertyChanged("StatusThumbsCollection");
        }
        #endregion
        #endregion

        #region Chats List
        #region Properties
        public ObservableCollection<ChatListData> mChats;
        public ObservableCollection<ChatListData> mPinnedChats;

        public ObservableCollection<ChatListData> Chats 
        {
            get => mChats;
            set {
                //to change the list
                if (mChats == value) return;

                //to update the list
                mChats = value;

                //updating filtered chats to match
                FilteredChats = new ObservableCollection<ChatListData>(mChats);
                OnPropertyChanged("Chats");
                OnPropertyChanged("FilteredChats");
            }
        }
        public ObservableCollection<ChatListData> PinnedChats
        {
            get => mPinnedChats;
            set
            {
                //to change the list
                if (mPinnedChats == value) return;

                //to update the list
                mPinnedChats = value;

                //updating filtered chats to match
                FilteredPinnedChats = new ObservableCollection<ChatListData>(mPinnedChats);
                OnPropertyChanged("PinnedChats");
                OnPropertyChanged("FilteredPinnedChats");
            }
        }
        protected ObservableCollection<ChatListData> _ArchiveChats;
        public ObservableCollection<ChatListData> ArchiveChats 
        { get => _ArchiveChats; 
            set { 
            _ArchiveChats = value;
                OnPropertyChanged();
            } }
        //Filtering Chats, Pinned chats 
        public ObservableCollection<ChatListData> FilteredChats { get; set; }
        public ObservableCollection<ChatListData> FilteredPinnedChats { get; set; }

        protected int ChatPosition { get; set; }
        #endregion

        #region Logics
        void LoadChats()
        {
            //loadind data from db
            if (Chats == null)
                Chats = new ObservableCollection<ChatListData>();
            //opening sql connection
            connection.Open();
            //temporary collection
            ObservableCollection<ChatListData> temp = new ObservableCollection<ChatListData>();
            using (SqlCommand command = new SqlCommand("select * from contacts p left join(select a.*, row_number() " +
                "over(partition by a.contactname order by a.id desc) as seqnum from conversations a) a on a.ContactName = p.contactname and a.seqnum = 1 order by a.id desc", connection))//редактировать запрос
            {
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    //to avoid duplication
                    string lastItem = string.Empty;
                    string newItem = string.Empty;

                    while (reader.Read())
                    {
                        string time = string.Empty;
                        string lastmessage = string.Empty;
                        //if the last message is recieved from sender than update time and lastmessage variables...
                        if (!string.IsNullOrEmpty(reader["MsgReceivedOn"].ToString()))
                        {
                            time = Convert.ToDateTime(reader["MsgReceivedOn"].ToString()).ToString("ddd hh:mm tt");
                            lastmessage = reader["ReceivedMsgs"].ToString();
                        }
                        //else if we have sent last msg then update accordingly...
                        if (!string.IsNullOrEmpty(reader["MsgSentOn"].ToString()))
                        {
                            time = Convert.ToDateTime(reader["MsgSentOn"].ToString()).ToString("ddd hh:mm tt");
                            lastmessage = reader["SentMsgs"].ToString();
                        }
                        //if the chat is new or we are starting new conversation which there will be no previous sent or recieved msgs in thst case...
                        //show star new conversation msg
                        if (string.IsNullOrEmpty(lastmessage))
                        {
                            lastmessage = "Start new conversation";

                        }
                        //udate data in model..
                        ChatListData chat = new ChatListData()
                        {
                            ContactPhoto = (byte[])reader["photo"],
                            ContactName = reader["contactname"].ToString(),
                            Message = lastmessage,
                            LastMessageTime = time
                        };

                        //update
                        newItem = reader["contactname"].ToString();
                        //if last added chat isn't same as new one then only add...
                        if (lastItem != newItem)
                            temp.Add(chat);
                        lastItem = newItem;
                    }
                }
            }
            //transfer data
            Chats = temp;
            //update
            OnPropertyChanged(nameof(Chats));

            //Chats = new ObservableCollection<ChatListData>()
            //{
            //    new ChatListData
            //    {
            //        ContactName="Билл",
            //        ContactPhoto=new Uri("/Assets/6.jpg",UriKind.RelativeOrAbsolute),
            //        Message="Привет, как дела?",
            //        LastMessageTime="вт, 15:28",
            //        ChatIsSelected=true
            //    },
            //    new ChatListData
            //    {
            //        ContactName="Майк",
            //        ContactPhoto=new Uri("/Assets/1.png",UriKind.RelativeOrAbsolute),
            //        Message="Проверь почту",
            //        LastMessageTime="пн, 12:01"
            //    },
            //    new ChatListData
            //    {
            //        ContactName="Стив",
            //        ContactPhoto=new Uri("/Assets/7.png",UriKind.RelativeOrAbsolute),
            //        Message="Да, было весело)",
            //        LastMessageTime="вт, 08:10"
            //    },
            //    new ChatListData
            //    {
            //        ContactName="Джон",
            //        ContactPhoto=new Uri("/Assets/8.jpg",UriKind.RelativeOrAbsolute),
            //        Message="Как ты?",
            //        LastMessageTime="вт, 01:00"
            //    }
            //};
            OnPropertyChanged();
        }
        #endregion
        #region Commands
        //to get the contactnaem of selected chat 
        protected ICommand _getSelectedChatCommand;

        public ICommand GetSelectedChatCommand => _getSelectedChatCommand ?? new RelayCommand(parameter =>
                    {
                        if (parameter is ChatListData v)
                        {
                            //getting contactname from selected chat
                            ContactName = v.ContactName;
                            OnPropertyChanged("ContactName");

                            //getting contactphoto from selected chat
                            ContactPhoto = v.ContactPhoto;
                            OnPropertyChanged("ContactPhoto");

                            LoadChatConversation(v);
                        }
                    });
        //to pin chat on pin button click
        protected ICommand _pinChatCommand;

        public ICommand PinChatCommand => _pinChatCommand ?? new RelayCommand(parameter =>
        {
            if (parameter is ChatListData v)
            {
                if (!FilteredPinnedChats.Contains(v))
                {

                    //add selected chat to pin chat
                    PinnedChats.Add(v);
                    FilteredPinnedChats.Add(v);
                    OnPropertyChanged("PinnedChats");
                    OnPropertyChanged("FilteredPinnedChats");
                    v.ChatIsPinned = true;
                    
                    //remove selected chat from all chats / unpinned chats
                    //store position of chat befor pinning sp that when we unoin or unarchive we get it on original position... 
                    ChatPosition = Chats.IndexOf(v);

                    Chats.Remove(v);
                    FilteredChats.Remove(v);
                    OnPropertyChanged("Chats");
                    OnPropertyChanged("FilteredChats");

                    if(ArchiveChats!=null)
                    {
                        if (ArchiveChats.Contains(v))
                        {
                            ArchiveChats.Remove(v);
                            v.ChatIsArchive = false;
                        }

                    }
                  
                }
            }
        });
        //to pin chat on pin button click
        protected ICommand _unPinChatCommand;

        public ICommand UnPinChatCommand => _unPinChatCommand ?? new RelayCommand(parameter =>
        {
            if (parameter is ChatListData v)
            {
                if (!FilteredPinnedChats.Contains(v))
                {
                    //restore position of chat befor pinning sp that when we unoin or unarchive we get it on original position... 
                   
                    //remove selected chat from all chats / unpinned chats
                    Chats.Add(v);
                    FilteredChats.Add(v);

                    //restore position of chat befor pinning sp that when we unoin or unarchive we get it on original position... 
                    Chats.Move(Chats.Count-1, ChatPosition);
                    FilteredChats.Move(Chats.Count-1, ChatPosition);
                    //update
                    OnPropertyChanged("Chats");
                    OnPropertyChanged("FilteredChats");
                    //add selected chat to pin chat
                    PinnedChats.Remove(v);
                    FilteredPinnedChats.Remove(v);
                    
                    OnPropertyChanged("PinnedChats");
                    OnPropertyChanged("FilteredPinnedChats");
                    v.ChatIsPinned = false;

                }
            }
        });

        protected ICommand _archiveChatCommand;
        public ICommand ArchiveChatCommand => _archiveChatCommand ?? new RelayCommand(parameter =>

        {
            if (parameter is ChatListData v)
            {
                if (!ArchiveChats.Contains(v))
                {

                  
                    //add chat in archive list
                    ArchiveChats.Add(v);
                    v.ChatIsArchive = true;
                    v.ChatIsPinned= false;
                    //remove chat from pinned and unpinned chat list 
                    Chats.Remove(v);
                    FilteredChats.Remove(v);
                    PinnedChats.Remove(v);
                    FilteredPinnedChats.Remove(v);


                    //update  list
                    OnPropertyChanged("Chats");
                    OnPropertyChanged("FilteredChats");                    
                    OnPropertyChanged("PinnedChats");
                    OnPropertyChanged("FilteredPinnedChats");
                    OnPropertyChanged("ArchiveChats");
                }
            }
        });

        protected ICommand _unArchiveChatCommand;
        public ICommand UnArchiveChatCommand => _unArchiveChatCommand ?? new RelayCommand(parameter =>

        {
            if (parameter is ChatListData v)
            {
                if (!FilteredChats.Contains(v) && !Chats.Contains(v))
                {

                    //remove chat from pinned and unpinned chat list 
                    Chats.Add(v);
                    FilteredChats.Add(v);
                }

                //add chat in archive list
                ArchiveChats.Remove(v);
                v.ChatIsArchive = false;
                v.ChatIsPinned = false;

                OnPropertyChanged("Chats");
                OnPropertyChanged("FilteredChats");
                OnPropertyChanged("ArchiveChats");
            }
            
        });
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
                //to change the list
                if (mConversations == value) return;

                //to update the list
                mConversations = value;

                //updating filtered chats to match
                FilteredConversations = new ObservableCollection<ChatConversation>(mConversations);
                OnPropertyChanged("Conversations");
                OnPropertyChanged("FilteredConversations");
            }
        }
        public ObservableCollection<ChatConversation> FilteredConversations { get; set; }

        protected string messageText;
        public string MessageText
        {
            get => messageText; 
            set
            {
                messageText = value;
                OnPropertyChanged("MessageText");
            }
        }

        protected string LastSearchConversationText;
        protected string mSearchConversationText;
        public string SearchConversationText
        {
            get => mSearchConversationText;
            set
            {
                //checked if value is different
                if (mSearchConversationText == value) { return; }
                //update view
                mSearchConversationText = value;
                //if search text is empty restore message
                if (string.IsNullOrEmpty(SearchConversationText)) SearchInConversation();
            }
        }
        public bool FocusMessageBox { get; set; }
        public bool IsThisAReplyMessage { get; set; }
        public string MessageToReplyText { get; set; }
        #endregion

        #region Logics
        protected bool _isConversationSearchBoxOpen;
        public bool IsConversationSearchBoxOpen
        {
            get => _isConversationSearchBoxOpen;
            set
            {
                if (_isConversationSearchBoxOpen == value) return;
                _isConversationSearchBoxOpen = value;
                if (_isConversationSearchBoxOpen == false) SearchConversationText = string.Empty;
                OnPropertyChanged("IsConversationSearchBoxOpen");
                OnPropertyChanged("SearchConversationText");
            }
        }
        public void OpenConversationSearchBox()
        {
            IsConversationSearchBoxOpen = true;
        }
        public void ClearConversationSearchBox()
        {
            if (!string.IsNullOrEmpty(SearchConversationText))
                SearchConversationText = string.Empty;
            else CloseConversationSearchBox();
        }
        public void CloseConversationSearchBox() => IsConversationSearchBoxOpen = false;

        private void LoadChatConversation( ChatListData chat)
        {
           
            
            if (connection.State == System.Data.ConnectionState.Closed)
                connection.Open();
            if(Conversations == null)
                Conversations = new ObservableCollection<ChatConversation>();
            Conversations.Clear();
            FilteredConversations.Clear();
            using (SqlCommand com = new SqlCommand("select * from conversations where ContactName=@ContactName", connection))
            {
                com.Parameters.AddWithValue("@ContactName",chat.ContactName);
                using (SqlDataReader reader = com.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string MsgReceivedOn = !string.IsNullOrEmpty(reader["MsgReceivedOn"].ToString()) ?
                            Convert.ToDateTime(reader["MsgReceivedOn"].ToString()).ToString("MMM dd, hh:mm tt") : "";
                        string MsgSentOn = !string.IsNullOrEmpty(reader["MsgSentOn"].ToString()) ?
    Convert.ToDateTime(reader["MsgSentOn"].ToString()).ToString("MMM dd, hh:mm tt") : "";

                        var conversation = new ChatConversation()
                        {
                            ContactName = reader["ContactName"].ToString(),
                            ReceivedMessage = reader["ReceivedMsgs"].ToString(),
                            MsgReceivedOn = MsgReceivedOn,
                            MsgSentOn = MsgSentOn,
                            SentMessage = reader["SentMsgs"].ToString(),
                            IsMessageReceived = !string.IsNullOrEmpty(reader["ReceivedMsgs"].ToString())
                        };
                        Conversations.Add(conversation);
                        OnPropertyChanged("Conversations");
                        FilteredConversations.Add(conversation);
                        OnPropertyChanged("FilteredConversations");

                        chat.Message = !string.IsNullOrEmpty(reader["ReceivedMsgs"].ToString())? reader["ReceivedMsgs"].ToString():
                            reader["SentMsgs"].ToString();
                    }
                }
            }
            //reset reply message text when the new chat is fetched
            MessageToReplyText=string.Empty;
            OnPropertyChanged("MessageToReplyText");
        }

        void SearchInConversation()
        {
            //to avoid searching same text again
            if ((string.IsNullOrEmpty(LastSearchConversationText) && string.IsNullOrEmpty(SearchConversationText)) || string.Equals(LastSearchConversationText, SearchConversationText))
            { return; }

            //if searchbox is empty or chat in null pr char cound less then 0
            if (string.IsNullOrEmpty(SearchConversationText) || Conversations == null || Conversations.Count <= 0)
            {
                FilteredConversations = new ObservableCollection<ChatConversation>(Conversations ?? Enumerable.Empty<ChatConversation>());
                OnPropertyChanged("FilteredConversations");

                //update last search text
                LastSearchConversationText = SearchConversationText;
                return;
            }

            //now? to find all chats that contain the text in our search box
            FilteredConversations = new ObservableCollection<ChatConversation>(
                Conversations.Where(
                    chat => chat.ReceivedMessage.ToLower().Contains(SearchConversationText)
                    ||
                    chat.SentMessage.ToLower().Contains(SearchConversationText)
                    ));
            OnPropertyChanged("FilteredConversations");
            //update last search text
            LastSearchConversationText = SearchConversationText;

        }

        public void CancelReply()
        {
            IsThisAReplyMessage = false;
            //reset reply msg text
            MessageToReplyText = string.Empty;
            OnPropertyChanged("MessageToReplyText");
        }
        public void SendMessage()
        {
           //send message only when the textbox is not empty
            if(!string.IsNullOrEmpty(MessageText))
            {
                var conversation = new ChatConversation()
                {
                    ReceivedMessage = MessageToReplyText,
                    SentMessage = MessageText,
                    MsgSentOn = DateTime.Now.ToString("MMM dd, hh:mm tt"),
                    MessageContainsReply = IsThisAReplyMessage
                };
                //add msg to conversation list
                FilteredConversations.Add(conversation);
                Conversations.Add(conversation);

                //clear message properties and textbox when message is sent
                MessageText=string.Empty;
                IsThisAReplyMessage = false;
                MessageToReplyText= string.Empty;
                UpdateChatAndMoveUp(Chats, conversation);
                UpdateChatAndMoveUp(PinnedChats, conversation);
                UpdateChatAndMoveUp(FilteredChats, conversation);
                UpdateChatAndMoveUp(FilteredPinnedChats, conversation);
                UpdateChatAndMoveUp(ArchiveChats, conversation);
                //update
                OnPropertyChanged("FilteredConversations");
                OnPropertyChanged("Conversations");
                OnPropertyChanged("MessageText");
                OnPropertyChanged("IsThisAReplyMessage");
                OnPropertyChanged("MessageToReplyText");
            }
        }
        //move the chat contact on top when new message is sent or recieved
        protected void UpdateChatAndMoveUp(ObservableCollection<ChatListData> chatList, ChatConversation conversation)
        {
            //check if the message sent is to selected contact or not
            var chat = chatList.FirstOrDefault(x=>x.ContactName == ContactName);
            //if found.. then
            if(chat != null)
            {
                //Update Contact Chat Last Message and Messahe Time..
                chat.Message = MessageText;
                chat.LastMessageTime = conversation.MsgSentOn;

                //move chat on top when new msg is recieved/sent...
                chatList.Move(chatList.IndexOf(chat), 0);
                //update collection
                OnPropertyChanged("Chats");
                OnPropertyChanged("PinnedChats");
                OnPropertyChanged("FilteredChats");
                OnPropertyChanged("FilteredPinnedChats");
                OnPropertyChanged("ArchiveChats");

            }
        }
        #endregion

        #region Commands
        protected ICommand _openConversationSearchCommand;
        public ICommand OpenConversationSearchCommand
        {
            get
            {
                if (_openConversationSearchCommand == null)
                    _openConversationSearchCommand = new CommandViewModel(OpenConversationSearchBox);
                return _openConversationSearchCommand;
            }

            set
            {
                _openConversationSearchCommand = value;
            }
        }
        protected ICommand _clearConversationSearchCommand;
        public ICommand ClearConversationSearchCommand
        {
            get
            {
                if (_clearConversationSearchCommand == null)
                    _clearConversationSearchCommand = new CommandViewModel(ClearConversationSearchBox);
                return _clearConversationSearchCommand;
            }

            set
            {
                _searchConversationCommand = value;
            }
        }
        protected ICommand _closeConversationSearchCommand;
        public ICommand CloseConversationSearchCommand
        {
            get
            {
                if (_closeConversationSearchCommand == null)
                    _closeConversationSearchCommand = new CommandViewModel(CloseConversationSearchBox);
                return _closeConversationSearchCommand;
            }

            set
            {
                _searchConversationCommand = value;
            }
        }
        protected ICommand _searchConversationCommand;
        public ICommand SearchConversationCommand
        {
            get
            {
                if (_searchConversationCommand == null)
                    _searchConversationCommand = new CommandViewModel(SearchInConversation);
                return _searchConversationCommand;
            }

            set
            {
                _searchConversationCommand = value;
            }
        }

        protected ICommand _replyCommand;

        public ICommand ReplyCommand => _replyCommand ?? new RelayCommand(parameter =>
        {
            if (parameter is ChatConversation v)
            {
                //if replying sender's message
                if (v.IsMessageReceived)
                    MessageToReplyText = v.ReceivedMessage;
                //if replying own message
                else
                    MessageToReplyText = v.SentMessage;
                //update
                OnPropertyChanged("MessageToReplyText");
                //set focus on message box when user clicks reply button
                FocusMessageBox = true;
                OnPropertyChanged("FocusMessageBox");
                //flag this msg as reply msg
                IsThisAReplyMessage = true;
                OnPropertyChanged("IsThisAReplyMessage");
            }
        });

        protected ICommand _cancelReplyCommand;
        public ICommand CancelReplyCommand
        {
            get
            {
                if (_cancelReplyCommand == null)
                    _cancelReplyCommand = new CommandViewModel(CancelReply);
                return _cancelReplyCommand;
            }

            set
            {
                _cancelReplyCommand = value;
            }
        }

        protected ICommand _sendMessageCommand;
        public ICommand SendMessageCommand
        {
            get
            {
                if (_sendMessageCommand == null)
                    _sendMessageCommand = new CommandViewModel(SendMessage);
                return _sendMessageCommand;
            }

            set
            {
                _sendMessageCommand = value;
            }
        }
        #endregion

        #endregion

        #region ContactInfo
        #region Properties
        protected bool _IsContactInfoOpen;
        public bool IsContactInfoOpen
        {
            get=> _IsContactInfoOpen;
            set { _IsContactInfoOpen = value;
                OnPropertyChanged("IsContactInfoOpen");
            }
        }

        #endregion

        #region Logics
        public void OpenContactInfo()=>IsContactInfoOpen=true;
        public void CloseContactInfo() => IsContactInfoOpen = false;
        #endregion

        #region Commands
        protected ICommand _openContactInfoCommand;
        public ICommand OpenContactInfoCommand
        {
            get
            {
                if (_openContactInfoCommand == null)
                    _openContactInfoCommand = new CommandViewModel(OpenContactInfo);
                return _openContactInfoCommand;
            }

            set
            {
                _openContactInfoCommand = value;
            }
        }

        protected ICommand _closeContactInfoCommand;
        public ICommand CloseContactInfoCommand
        {
            get
            {
                if (_closeContactInfoCommand == null)
                    _closeContactInfoCommand = new CommandViewModel(CloseContactInfo);
                return _closeContactInfoCommand;
            }

            set
            {
                _closeContactInfoCommand = value;
            }
        }
        #endregion
        #endregion

        SqlConnection connection = new SqlConnection(@"Data Source=LAPTOP-N9J4H06Q;Initial Catalog=Database;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");
        public ViewModel()
        {
            LoadStatusThumbs();
            LoadChats();
            
            PinnedChats = new ObservableCollection<ChatListData>();
            ArchiveChats = new ObservableCollection<ChatListData>();
        }
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this,new PropertyChangedEventArgs(propertyName));
        }

       




    }
}
