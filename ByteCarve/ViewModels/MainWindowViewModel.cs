using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace ByteCarve.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    [ObservableProperty] private ViewModelBase current_page;

    public MainWindowViewModel()
    {
        current_page = this;
    }

    [RelayCommand]
    public void Gotopick()
    {
        Current_page = new PickViewModel();
    }
}
