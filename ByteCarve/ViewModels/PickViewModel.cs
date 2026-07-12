using System.Drawing;
using Avalonia.Platform.Storage;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Linq;
using System.Threading.Tasks;
namespace ByteCarve.ViewModels;
public partial class PickViewModel : ViewModelBase
{
    [ObservableProperty] public string pn = null,error_txt=""; 
    [ObservableProperty] public bool error; 
    [ObservableProperty] private string? selectedFilePath="";
    private ByteCarve.ViewModels.MainWindowViewModel _main;
    
    public IStorageProvider? StorageProvider { get; set; }


    public PickViewModel(ByteCarve.ViewModels.MainWindowViewModel s)
    {
        _main = s;
    }
    [RelayCommand]
    private async Task ChooseFile()
    {
        if (StorageProvider is null) return;
        var files = await StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions
        {
            Title = "select a file twin",
            AllowMultiple = false
        });
        var file = files.FirstOrDefault();
        if (file is not null)
            SelectedFilePath = file.Path.LocalPath;
    }

    [RelayCommand]
    public void nex()
    {
        if (SelectedFilePath=="" || Pn=="")
        {
            Error = true;
            Error_txt="fill all fields please";
            return;
        }
        if (SelectedFilePath.Contains("."))
        {
            if (SelectedFilePath.Contains(".bin"))
            {
                _main.Path = SelectedFilePath;
                _main.Current_page=new ConfigsViewModel(_main);
            }
            else
            {
                Error = true;
                Error_txt="The app requires a binary file (disk image, dump, or raw .bin) to carve from.";
                return;
            }
            
        }

        _main.Path = SelectedFilePath;
        _main.Current_page=new ConfigsViewModel(_main);
    }
}