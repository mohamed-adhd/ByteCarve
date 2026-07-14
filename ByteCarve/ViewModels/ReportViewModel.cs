using CommunityToolkit.Mvvm.ComponentModel;
using Avalonia.Platform.Storage;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
namespace ByteCarve.ViewModels;
using ByteCarve.Models;

public partial class ReportViewModel : ViewModelBase
{
    private database sql= new database();
    private MainWindowViewModel _main;
    [ObservableProperty] private string pn,dr,op;
    [ObservableProperty] private int totalfiles=-1,sec;

    public ReportViewModel(MainWindowViewModel s,int dur,int tot)
    {
        _main = s;
        Pn = _main.Daname;
        Dr = TimeSpan.FromSeconds(dur).ToString(@"mm\:ss");
        sec = dur;
        Op=_main.Op;
        Totalfiles = tot;
    }

    [RelayCommand]
    public void Save()
    {
        database.ops temp = new database.ops();
        temp.name = pn;
        temp.a_name = _main.Daname;
        temp.images = Totalfiles;
        temp.type = _main.Datype;
        temp.dur = sec;
        sql.add(temp);
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