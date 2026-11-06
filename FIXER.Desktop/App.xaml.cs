using System.IO;
using System.Windows;
using FIXER.Application.UseCases;
using FIXER.Domain.Interfaces;
using FIXER.Infrastructure.Persistence;
using FIXER.Infrastructure.Persistence.Repositories;
using FIXER.InterfaceAdapter.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace FIXER.Desktop;

public partial class App : System.Windows.Application
{
    private ServiceProvider? _serviceProvider;

    protected override async void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        var services = new ServiceCollection();
        ConfigureServices(services);

        _serviceProvider = services.BuildServiceProvider();

        // Inicializar base de datos SQLite y seed inicial (§7.1 y §7.2)
        using (var scope = _serviceProvider.CreateScope())
        {
            var context = scope.ServiceProvider.GetRequiredService<FixerDbContext>();
            await DbInitializer.SeedAsync(context);
        }

        var mainWindow = _serviceProvider.GetRequiredService<MainWindow>();
        mainWindow.Show();
    }

    private void ConfigureServices(IServiceCollection services)
    {
        // 1. Configuración de Base de Datos SQLite Local
        string dbPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "fixer_gestion.db");
        services.AddDbContext<FixerDbContext>(options =>
            options.UseSqlite($"Data Source={dbPath}"));

        // 2. Repositorios de Infraestructura
        services.AddScoped<IProductoRepository, ProductoRepository>();
        services.AddScoped<ICategoriaRepository, CategoriaRepository>();

        // 3. Casos de Uso de Aplicación
        services.AddScoped<ObtenerCatalogoPaginadoUseCase>();
        services.AddScoped<GuardarProductoUseCase>();
        services.AddScoped<CambiarEstadoProductoUseCase>();
        services.AddScoped<RegistrarMermaUseCase>();
        services.AddScoped<ObtenerCategoriasUseCase>();
        services.AddScoped<ObtenerProveedoresUseCase>();

        // 4. ViewModels de Adaptadores de Interfaz
        services.AddTransient<CatalogoViewModel>();
        services.AddTransient<ProductoFormViewModel>();
        services.AddTransient<RegistrarMermaViewModel>();

        // 5. Ventana Principal
        services.AddTransient<MainWindow>();
    }

    protected override void OnExit(ExitEventArgs e)
    {
        _serviceProvider?.Dispose();
        base.OnExit(e);
    }
}
