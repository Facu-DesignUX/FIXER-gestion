using FIXER.Application.DTOs;
using FIXER.Domain.Entities;
using FIXER.Domain.Interfaces;

namespace FIXER.Application.UseCases;

public class RegistrarMermaUseCase
{
    private readonly IProductoRepository _productoRepository;

    public RegistrarMermaUseCase(IProductoRepository productoRepository)
    {
        _productoRepository = productoRepository;
    }

    public async Task EjecutarAsync(RegistrarMermaDto dto)
    {
        var producto = await _productoRepository.GetByIdAsync(dto.IdProducto);
        if (producto == null)
            throw new InvalidOperationException("El producto no existe.");

        if (!producto.EstadoActivo)
            throw new InvalidOperationException("No se pueden registrar mermas en productos inactivos.");

        // Descuenta stock y valida regla de justificación
        producto.RegistrarMerma(dto.Cantidad, dto.Justificacion);

        var merma = new Merma(dto.IdProducto, dto.Cantidad, dto.Justificacion);

        await _productoRepository.UpdateAsync(producto);
        await _productoRepository.AddMermaAsync(merma);
    }
}

