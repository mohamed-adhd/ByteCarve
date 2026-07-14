using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace ByteCarve.ViewModels;
using ByteCarve.Models;
using System.Collections.Generic;
public partial class HistoryViewModel: ViewModelBase
{
    private ByteCarve.ViewModels.MainWindowViewModel _main;
    [ObservableProperty] private List<database.ops> history=null;

    public HistoryViewModel(MainWindowViewModel s)
    {
        _main = s;
    }
    [RelayCommand]
    public void Back2m()
    {
        _main.Current_page = new PickViewModel(_main);
    }
}