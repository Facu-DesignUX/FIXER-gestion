using FIXER.Application.DTOs;
using FIXER.Domain.Entities;
using FIXER.Domain.Interfaces;

namespace FIXER.Application.UseCases;

public class ObtenerCatalogoPaginadoUseCase
{
    private readonly IProductoRepository _productoRepository;

    public ObtenerCatalogoPaginadoUseCase(IProductoRepository productoRepository)
    {
        _productoRepository = productoRepository;
    }

    public async Task<ResultadoPaginado<ProductoDto>> EjecutarAsync(
        int pageNumber,
        int pageSize,
        string? searchTerm = null,
        int? idCategoria = null,
        string? proveedor = null,
        bool soloActivos = true)
    {
        var (items, totalCount) = await _productoRepository.GetPagedAsync(
            pageNumber,
            pageSize,
            searchTerm,
            idCategoria,
            proveedor,
            soloActivos);

        var dtos = items.Select(p => new ProductoDto
        {
            IdProducto = p.IdProducto,
            Nombre = p.Nombre,
            PrecioVenta = p.PrecioVenta,
            Costo = p.Costo,
            Stock = p.Stock,
            StockMinimo = p.StockMinimo,
            Proveedor = p.Proveedor,
            UbicacionFisica = p.UbicacionFisica,
            UnidadMedida = p.UnidadMedida,
            EstadoActivo = p.EstadoActivo,
            IdCategoria = p.IdCategoria,
            NombreCategoria = p.Categoria?.NombreCategoria ?? "Sin categoría"
        });

        return new ResultadoPaginado<ProductoDto>
        {
            Items = dtos,
            TotalCount = totalCount,
            PageNumber = pageNumber,
            PageSize = pageSize
        };
    }
}

