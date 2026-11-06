using FIXER.Domain.Entities;
using FIXER.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FIXER.Infrastructure.Persistence.Repositories;

public class ProductoRepository : IProductoRepository
{
    private readonly FixerDbContext _context;

    public ProductoRepository(FixerDbContext context)
    {
        _context = context;
    }

    public async Task<Producto?> GetByIdAsync(int id)
    {
        return await _context.Productos
            .Include(p => p.Categoria)
            .FirstOrDefaultAsync(p => p.IdProducto == id);
    }

    public async Task<IEnumerable<Producto>> GetAllAsync(bool soloActivos = false)
    {
        var query = _context.Productos.Include(p => p.Categoria).AsQueryable();
        if (soloActivos)
            query = query.Where(p => p.EstadoActivo);

        return await query.OrderBy(p => p.Nombre).ToListAsync();
    }

    public async Task<(IEnumerable<Producto> Items, int TotalCount)> GetPagedAsync(
        int pageNumber,
        int pageSize,
        string? searchTerm = null,
        int? idCategoria = null,
        string? proveedor = null,
        bool soloActivos = true)
    {
        var query = _context.Productos
            .Include(p => p.Categoria)
            .AsQueryable();

        if (soloActivos)
        {
            query = query.Where(p => p.EstadoActivo);
        }

        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            var term = searchTerm.Trim().ToLower();
            query = query.Where(p => 
                p.Nombre.ToLower().Contains(term) || 
                p.IdProducto.ToString().Contains(term) ||
                p.UbicacionFisica.ToLower().Contains(term));
        }

        if (idCategoria.HasValue && idCategoria.Value > 0)
        {
            query = query.Where(p => p.IdCategoria == idCategoria.Value);
        }

        if (!string.IsNullOrWhiteSpace(proveedor) && proveedor != "Todos")
        {
            query = query.Where(p => p.Proveedor == proveedor);
        }

        var totalCount = await query.CountAsync();

        var items = await query
            .OrderBy(p => p.Nombre)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (items, totalCount);
    }

    public async Task<IEnumerable<string>> GetProveedoresAsync()
    {
        return await _context.Productos
            .Where(p => !string.IsNullOrEmpty(p.Proveedor))
            .Select(p => p.Proveedor)
            .Distinct()
            .OrderBy(p => p)
            .ToListAsync();
    }

    public async Task AddAsync(Producto producto)
    {
        await _context.Productos.AddAsync(producto);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Producto producto)
    {
        _context.Productos.Update(producto);
        await _context.SaveChangesAsync();
    }

    public async Task AddMermaAsync(Merma merma)
    {
        await _context.Mermas.AddAsync(merma);
        await _context.SaveChangesAsync();
    }
}

