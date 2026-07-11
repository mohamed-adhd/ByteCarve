using CommunityToolkit.Mvvm.ComponentModel;
using ByteCarve.Models;
namespace ByteCarve.ViewModels;
using ByteCarve.ViewModels;
public partial class CarvingProgressViewModel : ViewModelBase
{
    [ObservableProperty] public string daname;
    [ObservableProperty] public string selectedmode;
    private carver cv;
    private MainWindowViewModel _main;
    public CarvingProgressViewModel(MainWindowViewModel main)
    {
        _main = main;
        daname=_main.Daname;
        selectedmode = _main.Sm;
        cv= new carver(_main.Path);
        cv.Carvethashi();
        cv.write(_main.Op);
    }

}