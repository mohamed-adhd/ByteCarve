using Avalonia.Platform.Storage;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Linq;
using System.Threading.Tasks;
namespace ByteCarve.ViewModels;
public partial class PickViewModel : ViewModelBase
{
    [ObservableProperty] private string? selectedFilePath;
    public IStorageProvider? StorageProvider { get; set; }
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
}