using FIXER.Application.DTOs;
using FIXER.Domain.Entities;
using FIXER.Domain.Interfaces;

namespace FIXER.Application.UseCases;

public class GuardarProductoUseCase
{
    private readonly IProductoRepository _productoRepository;
    private readonly ICategoriaRepository _categoriaRepository;

    public GuardarProductoUseCase(
        IProductoRepository productoRepository,
        ICategoriaRepository categoriaRepository)
    {
        _productoRepository = productoRepository;
        _categoriaRepository = categoriaRepository;
    }

    public async Task<int> EjecutarAsync(GuardarProductoDto dto)
    {
        var categoria = await _categoriaRepository.GetByIdAsync(dto.IdCategoria);
        if (categoria == null)
            throw new InvalidOperationException("La categoría seleccionada no existe.");

        if (dto.IdProducto == 0)
        {
            // Alta
            var nuevoProducto = new Producto(
                dto.Nombre,
                dto.PrecioVenta,
                dto.Costo,
                dto.Stock,
                dto.StockMinimo,
                dto.Proveedor,
                dto.UbicacionFisica,
                dto.UnidadMedida,
                dto.IdCategoria);

            await _productoRepository.AddAsync(nuevoProducto);
            return nuevoProducto.IdProducto;
        }
        else
        {
            // Modificación
            var productoExistente = await _productoRepository.GetByIdAsync(dto.IdProducto);
            if (productoExistente == null)
                throw new InvalidOperationException("El producto a modificar no existe.");

            productoExistente.Actualizar(
                dto.Nombre,
                dto.PrecioVenta,
                dto.Costo,
                dto.StockMinimo,
                dto.Proveedor,
                dto.UbicacionFisica,
                dto.UnidadMedida,
                dto.IdCategoria);

            await _productoRepository.UpdateAsync(productoExistente);
            return productoExistente.IdProducto;
        }
    }
}

