using FIXER.Domain.Entities;

namespace FIXER.Domain.Interfaces;

public interface ICategoriaRepository
{
    Task<IEnumerable<Categoria>> GetAllAsync(bool soloActivas = true);
    Task<Categoria?> GetByIdAsync(int id);
    Task AddAsync(Categoria categoria);
    Task UpdateAsync(Categoria categoria);
    Task AplicarAumentoMasivoPorcentajeAsync(int? idCategoria, string? proveedor, decimal porcentaje);
}

