using Fatura;
using Fatura.Infrastructure;
using Fatura.Models;
using Fatura.Models.Catalogos;
using Fatura.Models.Enums;
using Fatura.Repositories.Implementations;
using Fatura.Repositories.Interfaces;
using Fatura.Services;
using Fatura.Services.Implementations;
using Fatura.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using QuestPDF.Infrastructure;
using System.Runtime.InteropServices;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews(options =>
{
    options.Filters.Add<EsAdminViewFilter>();
})
    .AddRazorOptions(options =>
    {
        options.ViewLocationExpanders.Add(new ClienteViewLocationExpander());
    });
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

QuestPDF.Settings.License = LicenseType.Community;

// Registrar DbContext con SQL Server
// Registrar DbContext con SQL Server
builder.Services.AddDbContext<xstoreContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    options.UseSqlServer(connectionString, sqlServerOptions =>
    {
        sqlServerOptions.CommandTimeout(120);
        sqlServerOptions.EnableRetryOnFailure(
            maxRetryCount: 3,
            maxRetryDelay: TimeSpan.FromSeconds(5),
            errorNumbersToAdd: null);
    });
});

// Registrar Repositories
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped<IFacturaRepository, FacturaRepository>();
builder.Services.AddScoped<IProductoRepository, ProductoRepository>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// Registrar Services
builder.Services.AddScoped<IFacturaService, FacturaService>();
builder.Services.AddScoped<IProductoService, ProductoService>();
builder.Services.AddScoped<ICategoriaService, CategoriaService>();
builder.Services.AddScoped<IMarcaService, MarcaService>();
builder.Services.AddScoped<IReporteService, ReporteService>();
builder.Services.AddScoped<IClienteService, ClienteService>();
builder.Services.AddScoped<IDashboardService, DashboardService>();
builder.Services.AddScoped<IUnidadMedidaService, UnidadMedidaService>();
builder.Services.AddScoped<IFacturaPdfService, FacturaPdfService>();
builder.Services.AddScoped<IQrService, QrService>();
builder.Services.AddScoped<IEmailService, EmailService>();

// Registrar FacturaTicketService solo en Windows (requiere System.Drawing.Printing)
if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
{
    builder.Services.AddScoped<IFacturaTicketService, FacturaTicketService>();
}
else
{
    // En plataformas no-Windows, registrar una implementación dummy o lanzar excepción
    builder.Services.AddScoped<IFacturaTicketService, FacturaTicketService>();
}



var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseSession();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// Seed de productos de inicio (Motos Rodriguez) si no hay ninguno
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<xstoreContext>();
    try
    {
        var tieneProductos = await db.Productos.AnyAsync();
        if (!tieneProductos)
        {
            var primeraUnidad = await db.UnidadMedidas.FirstOrDefaultAsync();
            if (primeraUnidad == null)
            {
                db.UnidadMedidas.Add(new UnidadMedida
                {
                    Nombre = "Unidad",
                    Abreviatura = "U",
                    Activo = true
                });
                await db.SaveChangesAsync();
                primeraUnidad = await db.UnidadMedidas.FirstAsync();
            }
            int? idUnidad = primeraUnidad?.IdUnidadMedida;

            var productosInicio = new[]
            {
                new Producto { NombreProducto = "KIT DE CILINDRO GLXXER150", Precio = 10.00m, Codigo = "KIT-GLXXER150", IdUnidadMedida = idUnidad, Tipo = TipoProducto.Producto, Activo = true },
                new Producto { NombreProducto = "KIT DE CILINDRO FZ20", Precio = 5.00m, Codigo = "KIT-FZ20", IdUnidadMedida = idUnidad, Tipo = TipoProducto.Producto, Activo = true },
                new Producto { NombreProducto = "KIT DE CILINDRO FZ16", Precio = 5.00m, Codigo = "KIT-FZ16", IdUnidadMedida = idUnidad, Tipo = TipoProducto.Producto, Activo = true },
                new Producto { NombreProducto = "PRENSA DE CLUTCH", Precio = 39.00m, Codigo = "PRENSA-CLUTCH", IdUnidadMedida = idUnidad, Tipo = TipoProducto.Producto, Activo = true }
            };
            await db.Productos.AddRangeAsync(productosInicio);
            await db.SaveChangesAsync();
        }
    }
    catch (Exception)
    {
        // Si falla (ej. BD no disponible), no bloquear el arranque
    }
}

app.Run();
