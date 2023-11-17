using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Navigation;

namespace app_interface.Models
{
    public class ChatListData: INotifyPropertyChanged
    {
        public string ContactName { get; set; }
        public Uri ContactPhoto { get; set; }
        protected string message;
        public string Message { 
            get 
            { 
                return message; 
            }
            set 
            {
                message = value;
                OnPropertyChanged();
            } 
        }
        protected string lastMessageTime;
        public string LastMessageTime 
        {
            get 
            {
                return lastMessageTime; 
            }

            set
            { 
                lastMessageTime = value;
                OnPropertyChanged();
            }
        }
        public bool ChatIsSelected { get; set; }
        public bool ChatIsPinned { get; set; }
        public bool ChatIsArchive { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}