using System.Windows;
using FIXER.InterfaceAdapter.ViewModels;

namespace FIXER.Desktop.Views;

public partial class RegistrarMermaWindow : Window
{
    public RegistrarMermaWindow(RegistrarMermaViewModel viewModel)
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

