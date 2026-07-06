using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace ByteCarve.ViewModels;

public partial class MainWindowViewModel : ObservableObject
{
    [ObservableProperty] private object current_page;

    public MainWindowViewModel()
    {
        current_page = this;
    }

    [RelayCommand]
    public void Gotopick()
    {
        current_page = new PickViewModel();
    }
}
