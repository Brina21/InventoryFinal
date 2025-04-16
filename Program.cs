using InventoryFinal.Data;
using InventoryFinal.Repository;
using InventoryFinal.Service;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

var builder = WebApplication.CreateBuilder(args);

// Configurar la cultura predeterminada
var cultureInfo = new CultureInfo("es-ES");
cultureInfo.NumberFormat.CurrencyDecimalSeparator = ",";
CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;
//

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped(typeof(IGenericoRepository<>), typeof(GenericoRepository<>));
builder.Services.AddScoped(typeof(GenericoService<>));

builder.Services.AddScoped(typeof(ProductoRepository));
builder.Services.AddScoped(typeof(ProductoService));

builder.Services.AddScoped(typeof(CompraRepository));
builder.Services.AddScoped(typeof(CompraService));

builder.Services.AddScoped(typeof(VentaRepository));
builder.Services.AddScoped(typeof(VentaService));

builder.Services.AddControllersWithViews();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();