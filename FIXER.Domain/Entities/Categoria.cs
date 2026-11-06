namespace FIXER.Domain.Entities;

public class Categoria
{
    public int IdCategoria { get; set; }
    public string NombreCategoria { get; set; } = string.Empty;
    public string? Descripcion { get; set; }
    public bool EstadoActivo { get; set; } = true;

    // Navegación
    public ICollection<Producto> Productos { get; set; } = new List<Producto>();

    public Categoria() { }

    public Categoria(string nombreCategoria, string? descripcion = null)
    {
        if (string.IsNullOrWhiteSpace(nombreCategoria))
            throw new ArgumentException("El nombre de la categoría no puede estar vacío.", nameof(nombreCategoria));

        NombreCategoria = nombreCategoria.Trim();
        Descripcion = descripcion?.Trim();
        EstadoActivo = true;
    }
}

