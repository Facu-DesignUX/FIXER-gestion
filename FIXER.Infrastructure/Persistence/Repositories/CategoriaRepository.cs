using FIXER.Domain.Entities;
using FIXER.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FIXER.Infrastructure.Persistence.Repositories;

public class CategoriaRepository : ICategoriaRepository
{
    private readonly FixerDbContext _context;

    public CategoriaRepository(FixerDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Categoria>> GetAllAsync(bool soloActivas = true)
    {
        var query = _context.Categorias.AsQueryable();
        if (soloActivas)
            query = query.Where(c => c.EstadoActivo);

        return await query.OrderBy(c => c.NombreCategoria).ToListAsync();
    }

    public async Task<Categoria?> GetByIdAsync(int id)
    {
        return await _context.Categorias.FindAsync(id);
    }

    public async Task AddAsync(Categoria categoria)
    {
        await _context.Categorias.AddAsync(categoria);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Categoria categoria)
    {
        _context.Categorias.Update(categoria);
        await _context.SaveChangesAsync();
    }

    public async Task AplicarAumentoMasivoPorcentajeAsync(int? idCategoria, string? proveedor, decimal porcentaje)
    {
        var query = _context.Productos.Where(p => p.EstadoActivo);

        if (idCategoria.HasValue && idCategoria.Value > 0)
        {
            query = query.Where(p => p.IdCategoria == idCategoria.Value);
        }

        if (!string.IsNullOrWhiteSpace(proveedor) && proveedor != "Todos")
        {
            query = query.Where(p => p.Proveedor == proveedor);
        }

        var productos = await query.ToListAsync();
        var multiplicador = 1 + (porcentaje / 100m);

        foreach (var p in productos)
        {
            p.PrecioVenta = Math.Round(p.PrecioVenta * multiplicador, 2);
        }

        await _context.SaveChangesAsync();
    }
}

