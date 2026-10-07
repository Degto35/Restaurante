using Microsoft.EntityFrameworkCore; 
using Restaurante.Web.Data;         
using Restaurante.Web.Services;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<DataContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("MyConnection")));
// NOTA: "DefaultConnection" debe ser el nombre exacto que pusiste en appsettings.json

// 2. Registrar tus Servicios (Interfaz -> Implementación)
builder.Services.AddScoped<ICategoriasService, CategoriasService>();
builder.Services.AddScoped<IPlatosService, PlatosService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();

//estamos añadiendo texto
//para probar que se guarden las modificaciones
//gracias :D
//gracias :D

//hola mundo
//hola
//poloooo marcoooo!!
//aqui estuve +

