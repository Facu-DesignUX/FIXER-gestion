using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FIXER.Application.DTOs;
using FIXER.Application.UseCases;

namespace FIXER.InterfaceAdapter.ViewModels;

public partial class ProductoFormViewModel : ObservableObject
{
    private readonly GuardarProductoUseCase _guardarProductoUseCase;
    private readonly ObtenerCategoriasUseCase _obtenerCategoriasUseCase;

    [ObservableProperty]
    private int _idProducto;

    [ObservableProperty]
    private string _nombre = string.Empty;

    [ObservableProperty]
    private decimal _precioVenta;

    [ObservableProperty]
    private decimal _costo;

    [ObservableProperty]
    private decimal _stock;

    [ObservableProperty]
    private decimal _stockMinimo = 5.0m;

    [ObservableProperty]
    private string _proveedor = string.Empty;

    [ObservableProperty]
    private string _ubicacionFisica = string.Empty;

    [ObservableProperty]
    private string _unidadMedida = "Unidad";

    [ObservableProperty]
    private CategoriaDto? _categoriaSeleccionada;

    [ObservableProperty]
    private ObservableCollection<CategoriaDto> _categorias = new();

    [ObservableProperty]
    private ObservableCollection<string> _unidadesMedida = new()
    {
        "Unidad", "Kg", "Gramos", "Metro", "Caja", "Bolsa"
    };

    [ObservableProperty]
    private string? _mensajeError;

    public bool EsModificacion => IdProducto > 0;
    public string TituloVentana => EsModificacion ? "Modificar Ficha de Producto" : "Nuevo Producto en Catálogo";

    public Action<bool>? OnCerrarVentana { get; set; }

    public ProductoFormViewModel(
        GuardarProductoUseCase guardarProductoUseCase,
        ObtenerCategoriasUseCase obtenerCategoriasUseCase)
    {
        _guardarProductoUseCase = guardarProductoUseCase;
        _obtenerCategoriasUseCase = obtenerCategoriasUseCase;
    }

    public async Task CargarCategoriasYDatosAsync(GuardarProductoDto? dto = null)
    {
        var lista = await _obtenerCategoriasUseCase.EjecutarAsync(soloActivas: true);
        Categorias.Clear();
        foreach (var c in lista)
        {
            Categorias.Add(c);
        }

        if (dto != null)
        {
            IdProducto = dto.IdProducto;
            Nombre = dto.Nombre;
            PrecioVenta = dto.PrecioVenta;
            Costo = dto.Costo;
            Stock = dto.Stock;
            StockMinimo = dto.StockMinimo;
            Proveedor = dto.Proveedor;
            UbicacionFisica = dto.UbicacionFisica;
            UnidadMedida = dto.UnidadMedida;
            CategoriaSeleccionada = Categorias.FirstOrDefault(c => c.IdCategoria == dto.IdCategoria);
        }
        else
        {
            IdProducto = 0;
            Nombre = string.Empty;
            PrecioVenta = 0;
            Costo = 0;
            Stock = 0;
            StockMinimo = 5.0m;
            Proveedor = string.Empty;
            UbicacionFisica = string.Empty;
            UnidadMedida = "Unidad";
            CategoriaSeleccionada = Categorias.FirstOrDefault();
        }

        OnPropertyChanged(nameof(EsModificacion));
        OnPropertyChanged(nameof(TituloVentana));
    }

    [RelayCommand]
    public async Task GuardarAsync()
    {
        MensajeError = null;

        if (string.IsNullOrWhiteSpace(Nombre))
        {
            MensajeError = "El nombre del producto no puede estar vacío.";
            return;
        }

        if (PrecioVenta < 0)
        {
            MensajeError = "El precio de venta debe ser mayor o igual a cero.";
            return;
        }

        if (Costo < 0)
        {
            MensajeError = "El costo no puede ser negativo.";
            return;
        }

        if (CategoriaSeleccionada == null || CategoriaSeleccionada.IdCategoria <= 0)
        {
            MensajeError = "Debe seleccionar una categoría válida.";
            return;
        }

        if (string.IsNullOrWhiteSpace(Proveedor))
        {
            MensajeError = "Debe indicar el distribuidor o proveedor.";
            return;
        }

        if (string.IsNullOrWhiteSpace(UbicacionFisica))
        {
            MensajeError = "Debe indicar la ubicación física (ej. Pasillo 1 - Estante B).";
            return;
        }

        // Validación de fraccionamiento
        if (string.Equals(UnidadMedida, "Unidad", StringComparison.OrdinalIgnoreCase))
        {
            if (Stock % 1 != 0 || StockMinimo % 1 != 0)
            {
                MensajeError = "Los artículos comercializados por 'Unidad' no admiten decimales en el stock.";
                return;
            }
        }

        try
        {
            var dto = new GuardarProductoDto
            {
                IdProducto = IdProducto,
                Nombre = Nombre,
                PrecioVenta = PrecioVenta,
                Costo = Costo,
                Stock = Stock,
                StockMinimo = StockMinimo,
                Proveedor = Proveedor,
                UbicacionFisica = UbicacionFisica,
                UnidadMedida = UnidadMedida,
                IdCategoria = CategoriaSeleccionada.IdCategoria
            };

            await _guardarProductoUseCase.EjecutarAsync(dto);
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

