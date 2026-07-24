using System;
using System.Diagnostics;
using CommunityToolkit.Mvvm.ComponentModel;
using ByteCarve.Models;
namespace ByteCarve.ViewModels;
using ByteCarve.ViewModels;
using System.Threading.Tasks;

public partial class CarvingProgressViewModel : ViewModelBase
{
    [ObservableProperty] public string daname;
    [ObservableProperty] public string selectedmode;

    private carver cv;
    private disassembler darksouls;
    private MainWindowViewModel _main;

    public CarvingProgressViewModel(MainWindowViewModel main)
    {
        _main = main;
        daname = _main.Daname;
        selectedmode = _main.Sm;
    }

    public async void Start()
    {
        if (_main.Datype == "media")
        {
            await Task.Yield();
            var sw = Stopwatch.StartNew();
            cv = new carver(_main.Path);
            int tot = cv.Carvethashi();
            cv.write(_main.Op);
            _main.Current_page = new ReportViewModel(_main, sw.Elapsed.Milliseconds, tot);
        }
        else
        {
            await Task.Yield();
            var sw = Stopwatch.StartNew();
            darksouls = new disassembler(_main.Path, _main.Op);
            darksouls.lord_have_mercy();
            _main.Current_page = new ReportViewModel(_main, sw.Elapsed.Milliseconds, 0);
        }
    }
}