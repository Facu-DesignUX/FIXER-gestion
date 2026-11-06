namespace FIXER.Domain.Entities;

public class Merma
{
    public int IdMerma { get; set; }
    public int IdProducto { get; set; }
    public decimal Cantidad { get; set; }
    public string Justificacion { get; set; } = string.Empty;
    public DateTime FechaHora { get; set; } = DateTime.Now;

    public Producto? Producto { get; set; }

    public Merma() { }

    public Merma(int idProducto, decimal cantidad, string justificacion)
    {
        if (idProducto <= 0)
            throw new ArgumentException("Producto inválido.");

        if (cantidad <= 0)
            throw new ArgumentException("La cantidad debe ser mayor a cero.");

        if (string.IsNullOrWhiteSpace(justificacion))
            throw new ArgumentException("La justificación escrita es obligatoria.", nameof(justificacion));

        IdProducto = idProducto;
        Cantidad = cantidad;
        Justificacion = justificacion.Trim();
        FechaHora = DateTime.Now;
    }
}

