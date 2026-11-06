namespace FIXER.Domain.Entities;

public class Producto
{
    public int IdProducto { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public decimal PrecioVenta { get; set; }
    public decimal Costo { get; set; }
    public decimal Stock { get; set; }
    public decimal StockMinimo { get; set; }
    public string Proveedor { get; set; } = string.Empty;
    public string UbicacionFisica { get; set; } = string.Empty; // Pasillo / Estante
    public string UnidadMedida { get; set; } = "Unidad"; // Unidad, Kg, Gramos, Metro, Caja, Bolsa
    public bool EstadoActivo { get; set; } = true;
    public int IdCategoria { get; set; }

    // Navegación
    public Categoria? Categoria { get; set; }

    public Producto() { }

    public Producto(
        string nombre,
        decimal precioVenta,
        decimal costo,
        decimal stock,
        decimal stockMinimo,
        string proveedor,
        string ubicacionFisica,
        string unidadMedida,
        int idCategoria)
    {
        ValidarDatos(nombre, precioVenta, costo, stock, stockMinimo, proveedor, ubicacionFisica, unidadMedida);

        Nombre = nombre.Trim();
        PrecioVenta = precioVenta;
        Costo = costo;
        Stock = stock;
        StockMinimo = stockMinimo;
        Proveedor = proveedor.Trim();
        UbicacionFisica = ubicacionFisica.Trim();
        UnidadMedida = unidadMedida.Trim();
        IdCategoria = idCategoria;
        EstadoActivo = true;
    }

    public void Actualizar(
        string nombre,
        decimal precioVenta,
        decimal costo,
        decimal stockMinimo,
        string proveedor,
        string ubicacionFisica,
        string unidadMedida,
        int idCategoria)
    {
        ValidarDatos(nombre, precioVenta, costo, Stock, stockMinimo, proveedor, ubicacionFisica, unidadMedida);

        Nombre = nombre.Trim();
        PrecioVenta = precioVenta;
        Costo = costo;
        StockMinimo = stockMinimo;
        Proveedor = proveedor.Trim();
        UbicacionFisica = ubicacionFisica.Trim();
        UnidadMedida = unidadMedida.Trim();
        IdCategoria = idCategoria;
    }

    public void DescontarStock(decimal cantidad)
    {
        ValidarFraccionamiento(cantidad);

        if (cantidad <= 0)
            throw new InvalidOperationException("La cantidad a descontar debe ser mayor a cero.");

        if (cantidad > Stock)
            throw new InvalidOperationException($"Stock insuficiente. Stock actual: {Stock}, solicitado: {cantidad}.");

        Stock -= cantidad;
    }

    public void AgregarStock(decimal cantidad)
    {
        ValidarFraccionamiento(cantidad);

        if (cantidad <= 0)
            throw new InvalidOperationException("La cantidad a ingresar debe ser mayor a cero.");

        Stock += cantidad;
    }

    public void RegistrarMerma(decimal cantidad, string justificacion)
    {
        if (string.IsNullOrWhiteSpace(justificacion))
            throw new ArgumentException("Es obligatorio ingresar una justificación escrita para registrar una merma.", nameof(justificacion));

        DescontarStock(cantidad);
    }

    public void DarDeBajaLogica()
    {
        EstadoActivo = false;
    }

    public void Reactivar()
    {
        EstadoActivo = true;
    }

    public bool EstaEnStockCritico()
    {
        return EstadoActivo && Stock <= StockMinimo;
    }

    public void ValidarFraccionamiento(decimal cantidad)
    {
        if (string.Equals(UnidadMedida, "Unidad", StringComparison.OrdinalIgnoreCase) && cantidad % 1 != 0)
        {
            throw new InvalidOperationException("Los productos comercializados por 'Unidad' no admiten cantidades fraccionadas (decimales).");
        }
    }

    private void ValidarDatos(
        string nombre,
        decimal precioVenta,
        decimal costo,
        decimal stock,
        decimal stockMinimo,
        string proveedor,
        string ubicacionFisica,
        string unidadMedida)
    {
        if (string.IsNullOrWhiteSpace(nombre))
            throw new ArgumentException("El nombre del producto es obligatorio.", nameof(nombre));

        if (precioVenta < 0)
            throw new ArgumentException("El precio de venta no puede ser negativo.", nameof(precioVenta));

        if (costo < 0)
            throw new ArgumentException("El costo no puede ser negativo.", nameof(costo));

        if (stock < 0)
            throw new ArgumentException("El stock no puede ser negativo.", nameof(stock));

        if (stockMinimo < 0)
            throw new ArgumentException("El stock mínimo no puede ser negativo.", nameof(stockMinimo));

        if (string.IsNullOrWhiteSpace(proveedor))
            throw new ArgumentException("El proveedor es obligatorio.", nameof(proveedor));

        if (string.IsNullOrWhiteSpace(ubicacionFisica))
            throw new ArgumentException("La ubicación física (pasillo/estante) es obligatoria.", nameof(ubicacionFisica));

        if (string.IsNullOrWhiteSpace(unidadMedida))
            throw new ArgumentException("La unidad de medida es obligatoria.", nameof(unidadMedida));

        if (string.Equals(unidadMedida, "Unidad", StringComparison.OrdinalIgnoreCase) && stock % 1 != 0)
            throw new ArgumentException("El stock inicial no puede tener decimales para unidad de medida 'Unidad'.");
    }
}

