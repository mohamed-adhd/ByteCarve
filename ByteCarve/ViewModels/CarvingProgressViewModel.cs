using System;
using System.Diagnostics;
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
        var sw=Stopwatch.StartNew();
        daname=_main.Daname;
        selectedmode = _main.Sm;
        cv= new carver(_main.Path);
        int tot=cv.Carvethashi();
        cv.write(_main.Op);
        Console.WriteLine($"seconds : {sw.Elapsed.Milliseconds}, total is : {tot}");
        _main.Current_page = new ReportViewModel(_main,sw.Elapsed.Milliseconds,tot);
        Console.WriteLine("reached after the report switch");

    }
}