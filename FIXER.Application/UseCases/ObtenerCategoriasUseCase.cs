using FIXER.Application.DTOs;
using FIXER.Domain.Interfaces;

namespace FIXER.Application.UseCases;

public class ObtenerCategoriasUseCase
{
    private readonly ICategoriaRepository _categoriaRepository;

    public ObtenerCategoriasUseCase(ICategoriaRepository categoriaRepository)
    {
        _categoriaRepository = categoriaRepository;
    }

    public async Task<IEnumerable<CategoriaDto>> EjecutarAsync(bool soloActivas = true)
    {
        var categorias = await _categoriaRepository.GetAllAsync(soloActivas);
        return categorias.Select(c => new CategoriaDto
        {
            IdCategoria = c.IdCategoria,
            NombreCategoria = c.NombreCategoria,
            Descripcion = c.Descripcion,
            EstadoActivo = c.EstadoActivo
        });
    }
}

