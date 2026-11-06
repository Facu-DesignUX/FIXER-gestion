using System.Windows;
using FIXER.Desktop.Views;
using FIXER.InterfaceAdapter.ViewModels;
using Microsoft.Extensions.DependencyInjection;

namespace FIXER.Desktop;

public partial class MainWindow : Window
{
    private readonly CatalogoViewModel _viewModel;
    private readonly IServiceProvider _serviceProvider;

    public MainWindow(CatalogoViewModel viewModel, IServiceProvider serviceProvider)
    {
        InitializeComponent();
        _viewModel = viewModel;
        _serviceProvider = serviceProvider;
        DataContext = _viewModel;

        ConfigurarCallbacks();

        Loaded += async (s, e) =>
        {
            await _viewModel.InicializarAsync();
        };
    }

    private void ConfigurarCallbacks()
    {
        // Diálogo para nuevo producto o edición
        _viewModel.OnSolicitarEdicionProducto = async (dto) =>
        {
            var formVm = _serviceProvider.GetRequiredService<ProductoFormViewModel>();
            await formVm.CargarCategoriasYDatosAsync(dto);

            var ventana = new ProductoFormWindow(formVm)
            {
                Owner = this
            };

            bool? res = ventana.ShowDialog();
            return res == true;
        };

        // Diálogo para registro de merma
        _viewModel.OnSolicitarRegistroMerma = (idProducto, nombre, stock) =>
        {
            var mermaVm = _serviceProvider.GetRequiredService<RegistrarMermaViewModel>();
            mermaVm.Configurar(idProducto, nombre, stock);

            var ventana = new RegistrarMermaWindow(mermaVm)
            {
                Owner = this
            };

            bool? res = ventana.ShowDialog();
            return Task.FromResult(res == true);
        };

        // Confirmaciones
        _viewModel.OnConfirmarAccion = (mensaje, titulo) =>
        {
            var resultado = MessageBox.Show(this, mensaje, titulo, MessageBoxButton.YesNo, MessageBoxImage.Question);
            return resultado == MessageBoxResult.Yes;
        };

        // Mensajes
        _viewModel.OnMostrarMensaje = (mensaje) =>
        {
            MessageBox.Show(this, mensaje, "FIXER Gestión", MessageBoxButton.OK, MessageBoxImage.Information);
        };
    }
}
