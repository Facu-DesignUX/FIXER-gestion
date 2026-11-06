using FIXER.Domain.Interfaces;

namespace FIXER.Application.UseCases;

public class CambiarEstadoProductoUseCase
{
    private readonly IProductoRepository _productoRepository;

    public CambiarEstadoProductoUseCase(IProductoRepository productoRepository)
    {
        _productoRepository = productoRepository;
    }

    public async Task EjecutarAsync(int idProducto)
    {
        var producto = await _productoRepository.GetByIdAsync(idProducto);
        if (producto == null)
            throw new InvalidOperationException("El producto no existe.");

        if (producto.EstadoActivo)
        {
            producto.DarDeBajaLogica();
        }
        else
        {
            producto.Reactivar();
        }

        await _productoRepository.UpdateAsync(producto);
    }
}

