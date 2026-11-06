using System.Windows;
using FIXER.InterfaceAdapter.ViewModels;

namespace FIXER.Desktop.Views;

public partial class ProductoFormWindow : Window
{
    public ProductoFormWindow(ProductoFormViewModel viewModel)
    {
        InitializeComponent();
        DataContext = viewModel;

        viewModel.OnCerrarVentana = (resultado) =>
        {
            DialogResult = resultado;
            Close();
        };
    }
}

