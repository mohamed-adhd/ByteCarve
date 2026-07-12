using CommunityToolkit.Mvvm.ComponentModel;
using Avalonia.Platform.Storage;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
namespace ByteCarve.ViewModels;

public partial class ReportViewModel : ViewModelBase
{
    private MainWindowViewModel _main;
    [ObservableProperty] private string pn,dr,op;
    [ObservableProperty] private int totalfiles=-1;

    public ReportViewModel(MainWindowViewModel s,int dur,int tot)
    {
        _main = s;
        Pn = _main.Daname;
        Dr = TimeSpan.FromSeconds(dur).ToString(@"mm\:ss");
        Op=_main.Op;
        Totalfiles = tot;
    }

    [RelayCommand]
    public void Save()
    {
        return;
    }

    [RelayCommand]
    public void Back2menu()
    {
        _main.Current_page = new PickViewModel(_main);
    }
    [RelayCommand]
    public void Opc()
    {
        var process = new Process
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = "thunar",
                Arguments = _main.Op,
                UseShellExecute = false
            }
        };
        process.Start();
    }
    

}