using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FIXER.Application.DTOs;
using FIXER.Application.UseCases;

namespace FIXER.InterfaceAdapter.ViewModels;

public partial class CatalogoViewModel : ObservableObject
{
    private readonly ObtenerCatalogoPaginadoUseCase _obtenerCatalogoUseCase;
    private readonly ObtenerCategoriasUseCase _obtenerCategoriasUseCase;
    private readonly ObtenerProveedoresUseCase _obtenerProveedoresUseCase;
    private readonly CambiarEstadoProductoUseCase _cambiarEstadoUseCase;

    [ObservableProperty]
    private ObservableCollection<ProductoDto> _productos = new();

    [ObservableProperty]
    private ObservableCollection<CategoriaDto> _categorias = new();

    [ObservableProperty]
    private ObservableCollection<string> _proveedores = new();

    [ObservableProperty]
    private ProductoDto? _productoSeleccionado;

    [ObservableProperty]
    private string _busquedaTexto = string.Empty;

    [ObservableProperty]
    private CategoriaDto? _categoriaFiltro;

    [ObservableProperty]
    private string _proveedorFiltro = "Todos";

    [ObservableProperty]
    private bool _soloActivos = true;

    [ObservableProperty]
    private int _paginaActual = 1;

    [ObservableProperty]
    private int _tamanoPagina = 20;

    [ObservableProperty]
    private int _totalItems;

    [ObservableProperty]
    private int _totalPaginas = 1;

    [ObservableProperty]
    private bool _cargando;

    [ObservableProperty]
    private string? _mensajeEstado;

    // Métricas del catálogo para el dashboard superior
    [ObservableProperty]
    private int _totalProductosActivos;

    [ObservableProperty]
    private int _totalStockCritico;

    public bool PuedeRetroceder => PaginaActual > 1 && !Cargando;
    public bool PuedeAvanzar => PaginaActual < TotalPaginas && !Cargando;

    public Func<GuardarProductoDto?, Task<bool>>? OnSolicitarEdicionProducto { get; set; }
    public Func<int, string, decimal, Task<bool>>? OnSolicitarRegistroMerma { get; set; }
    public Func<string, string, bool>? OnConfirmarAccion { get; set; }
    public Action<string>? OnMostrarMensaje { get; set; }

    public CatalogoViewModel(
        ObtenerCatalogoPaginadoUseCase obtenerCatalogoUseCase,
        ObtenerCategoriasUseCase obtenerCategoriasUseCase,
        ObtenerProveedoresUseCase obtenerProveedoresUseCase,
        CambiarEstadoProductoUseCase cambiarEstadoUseCase)
    {
        _obtenerCatalogoUseCase = obtenerCatalogoUseCase;
        _obtenerCategoriasUseCase = obtenerCategoriasUseCase;
        _obtenerProveedoresUseCase = obtenerProveedoresUseCase;
        _cambiarEstadoUseCase = cambiarEstadoUseCase;
    }

    public async Task InicializarAsync()
    {
        await CargarFiltrosAsync();
        await CargarProductosAsync();
    }

    [RelayCommand]
    public async Task CargarFiltrosAsync()
    {
        var categorias = await _obtenerCategoriasUseCase.EjecutarAsync(soloActivas: true);
        Categorias.Clear();
        Categorias.Add(new CategoriaDto { IdCategoria = 0, NombreCategoria = "Todas las categorías" });
        foreach (var c in categorias)
        {
            Categorias.Add(c);
        }
        CategoriaFiltro = Categorias.FirstOrDefault();

        var proveedores = await _obtenerProveedoresUseCase.EjecutarAsync();
        Proveedores.Clear();
        Proveedores.Add("Todos");
        foreach (var p in proveedores)
        {
            Proveedores.Add(p);
        }
        ProveedorFiltro = "Todos";
    }

    [RelayCommand]
    public async Task CargarProductosAsync()
    {
        if (Cargando) return;

        try
        {
            Cargando = true;
            MensajeEstado = "Cargando artículos...";

            int? idCat = (CategoriaFiltro != null && CategoriaFiltro.IdCategoria > 0) ? CategoriaFiltro.IdCategoria : null;
            string? prov = (ProveedorFiltro != "Todos") ? ProveedorFiltro : null;

            var resultado = await _obtenerCatalogoUseCase.EjecutarAsync(
                PaginaActual,
                TamanoPagina,
                BusquedaTexto,
                idCat,
                prov,
                SoloActivos);

            Productos.Clear();
            foreach (var item in resultado.Items)
            {
                Productos.Add(item);
            }

            TotalItems = resultado.TotalCount;
            TotalPaginas = Math.Max(1, resultado.TotalPages);

            // Métricas
            TotalProductosActivos = Productos.Count(p => p.EstadoActivo);
            TotalStockCritico = Productos.Count(p => p.EstaEnStockCritico);

            OnPropertyChanged(nameof(PuedeRetroceder));
            OnPropertyChanged(nameof(PuedeAvanzar));
            MensajeEstado = $"Mostrando {Productos.Count} de {TotalItems} artículos.";
        }
        catch (Exception ex)
        {
            MensajeEstado = $"Error: {ex.Message}";
            OnMostrarMensaje?.Invoke($"Ocurrió un error al cargar el catálogo: {ex.Message}");
        }
        finally
        {
            Cargando = false;
        }
    }

    [RelayCommand]
    public async Task PaginaSiguienteAsync()
    {
        if (PuedeAvanzar)
        {
            PaginaActual++;
            await CargarProductosAsync();
        }
    }

    [RelayCommand]
    public async Task PaginaAnteriorAsync()
    {
        if (PuedeRetroceder)
        {
            PaginaActual--;
            await CargarProductosAsync();
        }
    }

    [RelayCommand]
    public async Task BuscarAsync()
    {
        PaginaActual = 1;
        await CargarProductosAsync();
    }

    [RelayCommand]
    public async Task LimpiarFiltrosAsync()
    {
        BusquedaTexto = string.Empty;
        CategoriaFiltro = Categorias.FirstOrDefault();
        ProveedorFiltro = "Todos";
        SoloActivos = true;
        PaginaActual = 1;
        await CargarProductosAsync();
    }

    [RelayCommand]
    public async Task NuevoProductoAsync()
    {
        if (OnSolicitarEdicionProducto != null)
        {
            var guardado = await OnSolicitarEdicionProducto(null);
            if (guardado)
            {
                await CargarFiltrosAsync();
                await CargarProductosAsync();
                OnMostrarMensaje?.Invoke("Producto registrado exitosamente en el catálogo.");
            }
        }
    }

    [RelayCommand]
    public async Task EditarProductoAsync(ProductoDto? producto)
    {
        var target = producto ?? ProductoSeleccionado;
        if (target == null)
        {
            OnMostrarMensaje?.Invoke("Seleccione un producto para editar.");
            return;
        }

        var dto = new GuardarProductoDto
        {
            IdProducto = target.IdProducto,
            Nombre = target.Nombre,
            PrecioVenta = target.PrecioVenta,
            Costo = target.Costo,
            Stock = target.Stock,
            StockMinimo = target.StockMinimo,
            Proveedor = target.Proveedor,
            UbicacionFisica = target.UbicacionFisica,
            UnidadMedida = target.UnidadMedida,
            IdCategoria = target.IdCategoria
        };

        if (OnSolicitarEdicionProducto != null)
        {
            var guardado = await OnSolicitarEdicionProducto(dto);
            if (guardado)
            {
                await CargarFiltrosAsync();
                await CargarProductosAsync();
                OnMostrarMensaje?.Invoke("Producto actualizado correctamente.");
            }
        }
    }

    [RelayCommand]
    public async Task CambiarEstadoAsync(ProductoDto? producto)
    {
        var target = producto ?? ProductoSeleccionado;
        if (target == null)
        {
            OnMostrarMensaje?.Invoke("Seleccione un producto para modificar su estado.");
            return;
        }

        string accion = target.EstadoActivo ? "dar de baja (inactivar)" : "reactivar";
        if (OnConfirmarAccion != null)
        {
            bool confirma = OnConfirmarAccion(
                $"¿Está seguro de que desea {accion} el producto '{target.Nombre}'?",
                "Confirmación de Estado");
            if (!confirma) return;
        }

        try
        {
            await _cambiarEstadoUseCase.EjecutarAsync(target.IdProducto);
            await CargarProductosAsync();
            OnMostrarMensaje?.Invoke($"El producto fue {(target.EstadoActivo ? "inactivado" : "reactivado")} correctamente.");
        }
        catch (Exception ex)
        {
            OnMostrarMensaje?.Invoke($"Error al cambiar estado: {ex.Message}");
        }
    }

    [RelayCommand]
    public async Task RegistrarMermaAsync(ProductoDto? producto)
    {
        var target = producto ?? ProductoSeleccionado;
        if (target == null)
        {
            OnMostrarMensaje?.Invoke("Seleccione un producto para registrar la merma.");
            return;
        }

        if (!target.EstadoActivo)
        {
            OnMostrarMensaje?.Invoke("No se pueden registrar mermas sobre productos inactivos.");
            return;
        }

        if (target.Stock <= 0)
        {
            OnMostrarMensaje?.Invoke("El producto no posee existencias disponibles para descontar por merma.");
            return;
        }

        if (OnSolicitarRegistroMerma != null)
        {
            var registrado = await OnSolicitarRegistroMerma(target.IdProducto, target.Nombre, target.Stock);
            if (registrado)
            {
                await CargarProductosAsync();
                OnMostrarMensaje?.Invoke("Merma asentada y stock descontado correctamente con justificación escrita.");
            }
        }
    }
}

