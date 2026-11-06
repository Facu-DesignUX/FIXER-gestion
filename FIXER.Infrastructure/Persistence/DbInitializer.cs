using FIXER.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace FIXER.Infrastructure.Persistence;

public static class DbInitializer
{
    public static async Task SeedAsync(FixerDbContext context)
    {
        await context.Database.EnsureCreatedAsync();

        if (await context.Categorias.AnyAsync())
            return; // Ya fue inicializada

        // 1. Categorías iniciales
        var catHerramientas = new Categoria("Herramientas Manuales", "Martillos, destornilladores, llaves y pinzas");
        var catFijaciones = new Categoria("Tornillería y Fijaciones", "Bulones, tarugos, tuercas y tornillos");
        var catPintureria = new Categoria("Pinturería y Adhesivos", "Esmaltes, látex, selladores y pinceles");
        var catPlomeria = new Categoria("Plomería y Cañerías", "Accesorios termofusión, caños y griferías");
        var catElectricidad = new Categoria("Electricidad e Iluminación", "Cables, fichas, llaves de luz y portalámparas");
        var catConstruccion = new Categoria("Materiales de Construcción", "Arena, cal, cemento y alambres");

        await context.Categorias.AddRangeAsync(catHerramientas, catFijaciones, catPintureria, catPlomeria, catElectricidad, catConstruccion);
        await context.SaveChangesAsync();

        // 2. Catálogo inicial representativo (+15 artículos con datos para pruebas de stock crítico y fraccionamiento)
        var productos = new List<Producto>
        {
            new Producto(
                nombre: "Martillo Galponero 16oz Mango Fibra",
                precioVenta: 18500.00m,
                costo: 12000.00m,
                stock: 15.000m,
                stockMinimo: 5.000m,
                proveedor: "Stanley",
                ubicacionFisica: "Pasillo 1 - Estante A",
                unidadMedida: "Unidad",
                idCategoria: catHerramientas.IdCategoria),

            new Producto(
                nombre: "Destornillador Phillips Imantado 6x100mm",
                precioVenta: 5200.00m,
                costo: 3100.00m,
                stock: 4.000m, // Stock crítico
                stockMinimo: 6.000m,
                proveedor: "Bahco",
                ubicacionFisica: "Pasillo 1 - Estante B",
                unidadMedida: "Unidad",
                idCategoria: catHerramientas.IdCategoria),

            new Producto(
                nombre: "Pinza Universal 8 Pulgadas Aislada",
                precioVenta: 14200.00m,
                costo: 9500.00m,
                stock: 8.000m,
                stockMinimo: 3.000m,
                proveedor: "Bahco",
                ubicacionFisica: "Pasillo 1 - Estante A",
                unidadMedida: "Unidad",
                idCategoria: catHerramientas.IdCategoria),

            new Producto(
                nombre: "Tornillo Autoperforante T1 8x1/2 (Caja x 100)",
                precioVenta: 4800.00m,
                costo: 2800.00m,
                stock: 2.000m, // Stock crítico
                stockMinimo: 5.000m,
                proveedor: "Tel",
                ubicacionFisica: "Pasillo 2 - Estante C",
                unidadMedida: "Caja",
                idCategoria: catFijaciones.IdCategoria),

            new Producto(
                nombre: "Tornillo Madera Fix 4.0 x 40mm (por Kg)",
                precioVenta: 8900.00m,
                costo: 5500.00m,
                stock: 18.750m, // Fraccionado
                stockMinimo: 10.000m,
                proveedor: "Tel",
                ubicacionFisica: "Pasillo 2 - Estante D",
                unidadMedida: "Kg",
                idCategoria: catFijaciones.IdCategoria),

            new Producto(
                nombre: "Tarugo Espiga Nro 8 con tope (Bolsa x 100)",
                precioVenta: 3200.00m,
                costo: 1800.00m,
                stock: 12.000m,
                stockMinimo: 4.000m,
                proveedor: "Fischer",
                ubicacionFisica: "Pasillo 2 - Estante C",
                unidadMedida: "Bolsa",
                idCategoria: catFijaciones.IdCategoria),

            new Producto(
                nombre: "Látex Interior Lavable Blanco 20 Lts",
                precioVenta: 72000.00m,
                costo: 48000.00m,
                stock: 3.000m, // Stock crítico
                stockMinimo: 4.000m,
                proveedor: "Alba",
                ubicacionFisica: "Pasillo 3 - Estante A",
                unidadMedida: "Unidad",
                idCategoria: catPintureria.IdCategoria),

            new Producto(
                nombre: "Esmalte Sintético Satinado Negro 1 Lt",
                precioVenta: 12500.00m,
                costo: 8200.00m,
                stock: 10.000m,
                stockMinimo: 3.000m,
                proveedor: "Alba",
                ubicacionFisica: "Pasillo 3 - Estante B",
                unidadMedida: "Unidad",
                idCategoria: catPintureria.IdCategoria),

            new Producto(
                nombre: "Silicona Neutra Transparente 280ml",
                precioVenta: 6100.00m,
                costo: 3900.00m,
                stock: 7.000m,
                stockMinimo: 5.000m,
                proveedor: "Sika",
                ubicacionFisica: "Pasillo 3 - Estante C",
                unidadMedida: "Unidad",
                idCategoria: catPintureria.IdCategoria),

            new Producto(
                nombre: "Caño Termofusión 20mm x 4 Metros",
                precioVenta: 6900.00m,
                costo: 4300.00m,
                stock: 25.000m,
                stockMinimo: 10.000m,
                proveedor: "Tigre",
                ubicacionFisica: "Patio Trasero - Sector 1",
                unidadMedida: "Unidad",
                idCategoria: catPlomeria.IdCategoria),

            new Producto(
                nombre: "Manguera Riego Reforzada 1/2 (por Metro)",
                precioVenta: 1450.00m,
                costo: 900.00m,
                stock: 6.500m, // Fraccionado y crítico
                stockMinimo: 20.000m,
                proveedor: "Ginyplas",
                ubicacionFisica: "Pasillo 4 - Estante A",
                unidadMedida: "Metro",
                idCategoria: catPlomeria.IdCategoria),

            new Producto(
                nombre: "Cable Unipolar 2.5mm Normalizado (por Metro)",
                precioVenta: 1100.00m,
                costo: 720.00m,
                stock: 142.300m, // Fraccionado
                stockMinimo: 50.000m,
                proveedor: "Prysmian",
                ubicacionFisica: "Pasillo 5 - Estante A",
                unidadMedida: "Metro",
                idCategoria: catElectricidad.IdCategoria),

            new Producto(
                nombre: "Lámpara LED Cálida 9W Rosca E27",
                precioVenta: 2100.00m,
                costo: 1200.00m,
                stock: 1.000m, // Stock crítico
                stockMinimo: 10.000m,
                proveedor: "Philips",
                ubicacionFisica: "Pasillo 5 - Estante B",
                unidadMedida: "Unidad",
                idCategoria: catElectricidad.IdCategoria),

            new Producto(
                nombre: "Cinta Aisladora Negra 20 Metros",
                precioVenta: 1800.00m,
                costo: 1050.00m,
                stock: 35.000m,
                stockMinimo: 10.000m,
                proveedor: "3M",
                ubicacionFisica: "Pasillo 5 - Estante C",
                unidadMedida: "Unidad",
                idCategoria: catElectricidad.IdCategoria),

            new Producto(
                nombre: "Alambre de Fardo Negro Nro 16 (por Kg)",
                precioVenta: 4200.00m,
                costo: 2600.00m,
                stock: 45.200m, // Fraccionado
                stockMinimo: 15.000m,
                proveedor: "Acindar",
                ubicacionFisica: "Patio Trasero - Sector 2",
                unidadMedida: "Kg",
                idCategoria: catConstruccion.IdCategoria)
        };

        await context.Productos.AddRangeAsync(productos);
        await context.SaveChangesAsync();
    }
}

