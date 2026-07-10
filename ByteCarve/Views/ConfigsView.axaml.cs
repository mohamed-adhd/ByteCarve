using Avalonia.Controls;
using ByteCarve.ViewModels;

namespace ByteCarve.Views;

public partial class ConfigsView : UserControl
{
    public ConfigsView()
    {
        InitializeComponent();
        Loaded += (_, _) =>
        {
            var topLevel = TopLevel.GetTopLevel(this);
            if (DataContext is ConfigsViewModel vm && topLevel is not null)
                vm.StorageProvider = topLevel.StorageProvider;
        };
    }
}
