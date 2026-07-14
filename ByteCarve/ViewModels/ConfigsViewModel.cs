using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Linq;
using Avalonia.Platform.Storage;
using System.Threading.Tasks;

namespace ByteCarve.ViewModels;

public partial class ConfigsViewModel : ViewModelBase
{
    [ObservableProperty] public string mode, error_t="";
    [ObservableProperty] public bool isMediaMode = false;
    [ObservableProperty] public bool isCodeMode = false, error = false;
    [ObservableProperty] public string oP="";
    private MainWindowViewModel _main;

    public ConfigsViewModel(MainWindowViewModel s)
    {
        _main = s;
    }
    public IStorageProvider? StorageProvider { get; set; }

    [RelayCommand]
    public void SelectMediaMode()
    {
        IsMediaMode = true;
    }
    [RelayCommand]
    public void SelectCodeMode()
    {
        IsCodeMode = true;
    }
    [RelayCommand]
    public async Task PickOutputFolder()
    {
        if (StorageProvider is null) return;
        var files = await StorageProvider.OpenFolderPickerAsync(new FolderPickerOpenOptions());
        var file = files.FirstOrDefault();
        if (file is not null)
            OP = file.Path.LocalPath;
        
    }

    [RelayCommand]
    public void Startit()
    {
        if (!IsCodeMode && !IsMediaMode)
        {
            Error = true;
            Error_t = "select a mode please!";
            return;
        }
        if (OP == "")
        {
            Error = true;
            Error_t = "set an output folder please!";
            return;
        }

        _main.Op = OP;
        if (IsCodeMode)
        {
            _main.Datype = "code";
        }
        else
        {
            _main.Datype = "media";
        }
        var progress = new CarvingProgressViewModel(_main);
        _main.Current_page = progress;
        progress.Start();
    }
}