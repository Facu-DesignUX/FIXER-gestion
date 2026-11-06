# FIXER gestión

## Funcionalidades del Módulo
Para esta entrega se implementó el módulo de Inventario y Catálogo de Productos con el siguiente funcionamiento:
- **Catálogo y Búsqueda Reactiva:** Búsqueda en tiempo real por texto (descripción/código) y filtro por categoría.
- **Paginación:** Resultados paginados a 20 registros por página para optimizar rendimiento de memoria.
- **Alertas Visuales:** Resaltado automático en la grilla para productos cuyo stock actual sea menor o igual al stock mínimo configurado.
- **Alta y Modificación de Productos:** Soporte para unidades enteras (unidades) y fraccionarias (kg, metros) con validaciones de negocio.
- **Baja y Alta Lógica:** Desactivación de productos sin borrado físico de la base de datos para preservar la integridad histórica.
- **Registro Auditado de Mermas:** Ajuste negativo de stock con motivo y justificación obligatoria.

---

## Tecnologías y Arquitectura

El proyecto está desarrollado bajo el estándar de **Clean Architecture (Arquitectura Limpia)** desacoplado en 5 proyectos:

1. **`FIXER.Domain`**: Entidades del negocio (`Producto`, `Categoria`, `Merma`), reglas de validación pura y contratos de repositorios (`IProductoRepository`, `ICategoriaRepository`). Sin dependencias externas.
2. **`FIXER.Application`**: Casos de uso (`ObtenerCatalogoPaginadoUseCase`, `GuardarProductoUseCase`, `CambiarEstadoProductoUseCase`, `RegistrarMermaUseCase`) y DTOs de transferencia de datos.
3. **`FIXER.Infrastructure`**: Persistencia con **Entity Framework Core 10** y base de datos local **SQLite**. Incluye configuración Fluent API y sembrado de datos inicial.
4. **`FIXER.InterfaceAdapter`**: ViewModels desacoplados de la vista utilizando **CommunityToolkit.Mvvm** (patrón MVVM, comandos y propiedades reactivas).
5. **`FIXER.Desktop`**: Interfaz de usuario de escritorio desarrollada en **WPF (.NET 10)** con contenedor de Inyección de Dependencias (`Microsoft.Extensions.DependencyInjection`).

---

## Cómo ejecutar

```bash
dotnet run --project FIXER.Desktop
```

> **Nota:** Al ejecutar por primera vez, el sistema autogenera la base de datos local SQLite (`fixer.db`) con un lote de 15 productos de prueba para poder evaluar las alertas visuales y las validaciones.
