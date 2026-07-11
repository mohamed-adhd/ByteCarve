using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Linq;
using Avalonia.Platform.Storage;
using System.Threading.Tasks;

namespace ByteCarve.ViewModels;

public partial class ConfigsViewModel : ViewModelBase
{
    [ObservableProperty] public string mode;
    [ObservableProperty] public bool isMediaMode = false;
    [ObservableProperty] public bool isCodeMode = false;
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
        if (!IsCodeMode || !IsMediaMode)
        {
            return;
        }

        if (OP == "")
        {
            return;
        }
        _main.Current_page= new CarvingProgressViewModel(_main);
    }
}