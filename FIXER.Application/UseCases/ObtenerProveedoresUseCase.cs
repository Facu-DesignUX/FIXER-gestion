using FIXER.Domain.Interfaces;

namespace FIXER.Application.UseCases;

public class ObtenerProveedoresUseCase
{
    private readonly IProductoRepository _productoRepository;

    public ObtenerProveedoresUseCase(IProductoRepository productoRepository)
    {
        _productoRepository = productoRepository;
    }

    public async Task<IEnumerable<string>> EjecutarAsync()
    {
        return await _productoRepository.GetProveedoresAsync();
    }
}

