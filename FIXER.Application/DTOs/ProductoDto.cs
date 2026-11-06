namespace FIXER.Application.DTOs;

public class ProductoDto
{
    public int IdProducto { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public decimal PrecioVenta { get; set; }
    public decimal Costo { get; set; }
    public decimal Stock { get; set; }
    public decimal StockMinimo { get; set; }
    public string Proveedor { get; set; } = string.Empty;
    public string UbicacionFisica { get; set; } = string.Empty;
    public string UnidadMedida { get; set; } = "Unidad";
    public bool EstadoActivo { get; set; }
    public int IdCategoria { get; set; }
    public string NombreCategoria { get; set; } = string.Empty;

    public bool EstaEnStockCritico => EstadoActivo && Stock <= StockMinimo;
    public string EstadoTexto => EstadoActivo ? "Activo" : "Inactivo";
    public string StockFormateado => string.Equals(UnidadMedida, "Unidad", StringComparison.OrdinalIgnoreCase) 
        ? ((int)Stock).ToString() 
        : Stock.ToString("0.###");
    public string StockMinimoFormateado => string.Equals(UnidadMedida, "Unidad", StringComparison.OrdinalIgnoreCase) 
        ? ((int)StockMinimo).ToString() 
        : StockMinimo.ToString("0.###");
}

public class CategoriaDto
{
    public int IdCategoria { get; set; }
    public string NombreCategoria { get; set; } = string.Empty;
    public string? Descripcion { get; set; }
    public bool EstadoActivo { get; set; }
}

public class GuardarProductoDto
{
    public int IdProducto { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public decimal PrecioVenta { get; set; }
    public decimal Costo { get; set; }
    public decimal Stock { get; set; }
    public decimal StockMinimo { get; set; }
    public string Proveedor { get; set; } = string.Empty;
    public string UbicacionFisica { get; set; } = string.Empty;
    public string UnidadMedida { get; set; } = "Unidad";
    public int IdCategoria { get; set; }
}

public class RegistrarMermaDto
{
    public int IdProducto { get; set; }
    public decimal Cantidad { get; set; }
    public string Justificacion { get; set; } = string.Empty;
}

public class ResultadoPaginado<T>
{
    public IEnumerable<T> Items { get; set; } = Enumerable.Empty<T>();
    public int TotalCount { get; set; }
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);
    public bool HasPreviousPage => PageNumber > 1;
    public bool HasNextPage => PageNumber < TotalPages;
}

