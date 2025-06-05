using InventoryFinal.Data;
using InventoryFinal.Models;
using InventoryFinal.Repository;
using InventoryFinal.Service;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Globalization;

var builder = WebApplication.CreateBuilder(args);

// Configurar la cultura predeterminada para los números decimales
var cultureInfo = new CultureInfo("en-US");
CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;

// Configuración del contexto y servicios
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped(typeof(IGenericoRepository<>), typeof(GenericoRepository<>));
builder.Services.AddScoped(typeof(GenericoService<>));

builder.Services.AddScoped<ProductoRepository>();
builder.Services.AddScoped<ProductoService>();

builder.Services.AddScoped<CompraRepository>();
builder.Services.AddScoped<CompraService>();

builder.Services.AddScoped<VentaRepository>();
builder.Services.AddScoped<VentaService>();

builder.Services.AddScoped<GenericoService<Cliente>>();

builder.Services.AddScoped<GenericoService<MovimientoStock>>();

// Configuración de sesión
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// Configuración de controladores con NewtonsoftJson (evitar bucles de referencia)
builder.Services.AddControllersWithViews()
    .AddNewtonsoftJson(options =>
        options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore);

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseSession();

app.UseAuthorization();

// Soporte para ÁREAS y RUTA POR DEFECTO
app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Dashboard}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();