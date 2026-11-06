using FIXER.Domain.Entities;

namespace FIXER.Domain.Interfaces;

public interface IProductoRepository
{
    Task<Producto?> GetByIdAsync(int id);
    Task<IEnumerable<Producto>> GetAllAsync(bool soloActivos = false);
    Task<(IEnumerable<Producto> Items, int TotalCount)> GetPagedAsync(
        int pageNumber,
        int pageSize,
        string? searchTerm = null,
        int? idCategoria = null,
        string? proveedor = null,
        bool soloActivos = true);
    Task<IEnumerable<string>> GetProveedoresAsync();
    Task AddAsync(Producto producto);
    Task UpdateAsync(Producto producto);
    Task AddMermaAsync(Merma merma);
}

