using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FIXER.Application.DTOs;
using FIXER.Application.UseCases;

namespace FIXER.InterfaceAdapter.ViewModels;

public partial class RegistrarMermaViewModel : ObservableObject
{
    private readonly RegistrarMermaUseCase _registrarMermaUseCase;

    [ObservableProperty]
    private int _idProducto;

    [ObservableProperty]
    private string _nombreProducto = string.Empty;

    [ObservableProperty]
    private decimal _stockDisponible;

    [ObservableProperty]
    private decimal _cantidadADescontar = 1.0m;

    [ObservableProperty]
    private string _justificacion = string.Empty;

    [ObservableProperty]
    private string? _mensajeError;

    public Action<bool>? OnCerrarVentana { get; set; }

    public RegistrarMermaViewModel(RegistrarMermaUseCase registrarMermaUseCase)
    {
        _registrarMermaUseCase = registrarMermaUseCase;
    }

    public void Configurar(int idProducto, string nombreProducto, decimal stockDisponible)
    {
        IdProducto = idProducto;
        NombreProducto = nombreProducto;
        StockDisponible = stockDisponible;
        CantidadADescontar = 1.0m;
        Justificacion = string.Empty;
        MensajeError = null;
    }

    [RelayCommand]
    public async Task ConfirmarMermaAsync()
    {
        MensajeError = null;

        if (CantidadADescontar <= 0)
        {
            MensajeError = "La cantidad a dar de baja debe ser mayor a cero.";
            return;
        }

        if (CantidadADescontar > StockDisponible)
        {
            MensajeError = $"La cantidad a descontar ({CantidadADescontar}) no puede superar el stock disponible ({StockDisponible}).";
            return;
        }

        if (string.IsNullOrWhiteSpace(Justificacion))
        {
            MensajeError = "Es obligatorio asentar el motivo o justificación por escrito de la merma.";
            return;
        }

        try
        {
            var dto = new RegistrarMermaDto
            {
                IdProducto = IdProducto,
                Cantidad = CantidadADescontar,
                Justificacion = Justificacion
            };

            await _registrarMermaUseCase.EjecutarAsync(dto);
            OnCerrarVentana?.Invoke(true);
        }
        catch (Exception ex)
        {
            MensajeError = ex.Message;
        }
    }

    [RelayCommand]
    public void Cancelar()
    {
        OnCerrarVentana?.Invoke(false);
    }
}

