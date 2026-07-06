using Avalonia.Controls;
using ByteCarve.ViewModels;
namespace ByteCarve.Views;
public partial class PickView : UserControl
{
    public PickView()
    {
        InitializeComponent();
        Loaded += (_, _) =>
        {
            var topLevel = TopLevel.GetTopLevel(this);
            if (DataContext is PickViewModel vm && topLevel is not null)
                vm.StorageProvider = topLevel.StorageProvider;
        };
    }
}