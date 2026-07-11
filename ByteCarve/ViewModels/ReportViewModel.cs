using CommunityToolkit.Mvvm.ComponentModel;
using Avalonia.Platform.Storage;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Linq;
using System.Threading.Tasks;
namespace ByteCarve.ViewModels;

public partial class ReportViewModel : ViewModelBase
{
    private MainWindowViewModel _main;
    [ObservableProperty] private string pn;

    public ReportViewModel(MainWindowViewModel s)
    {
        _main = s;
        Pn = _main.Daname;
        
        

    }

}