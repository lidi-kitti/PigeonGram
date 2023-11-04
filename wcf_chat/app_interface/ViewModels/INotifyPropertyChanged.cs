using System.Windows.Input;

namespace app_interface.ViewModels
{
    public interface INotifyPropertyChanged
    {
        ICommand GetSelectedChatCommand { get; }
    }
}