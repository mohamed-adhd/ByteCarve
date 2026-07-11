using CommunityToolkit.Mvvm.ComponentModel;

namespace ByteCarve.ViewModels;
using ByteCarve.ViewModels;
public partial class CarvingProgressViewModel : ViewModelBase
{
    [ObservableProperty] public string daname;
    [ObservableProperty] public string selectedmode;
    

    private MainWindowViewModel _main;
    
    public CarvingProgressViewModel(MainWindowViewModel main)
    {
        _main = main;
        daname=_main.Daname;
        selectedmode = _main.Sm;
    }

}