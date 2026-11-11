using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Restaurante.Web.Data;
using Restaurante.Web.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<DataContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<PasswordService>();
builder.Services.AddScoped<AuthenticationService>();

builder.Services.AddScoped<IAuthorizationHandler, PermissionAuthorizationHandler>();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Accounts/Login";
        options.AccessDeniedPath = "/Accounts/AccessDenied";
        options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
        options.SlidingExpiration = true;
    });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Usuarios.Ver",
        policy => policy.Requirements.Add(
            new PermissionRequirement("Usuarios.Ver")));

    options.AddPolicy("Usuarios.Crear",
        policy => policy.Requirements.Add(
            new PermissionRequirement("Usuarios.Crear")));

    options.AddPolicy("Usuarios.Editar",
        policy => policy.Requirements.Add(
            new PermissionRequirement("Usuarios.Editar")));

    options.AddPolicy("Roles.Ver",
        policy => policy.Requirements.Add(
            new PermissionRequirement("Roles.Ver")));

    options.AddPolicy("Roles.Crear",
        policy => policy.Requirements.Add(
            new PermissionRequirement("Roles.Crear")));

    options.AddPolicy("Roles.Editar",
        policy => policy.Requirements.Add(
            new PermissionRequirement("Roles.Editar")));

    options.AddPolicy("Cuentas.Ver",
        policy => policy.Requirements.Add(
            new PermissionRequirement("Cuentas.Ver")));
});

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Accounts}/{action=Login}/{id?}");

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<DataContext>();
    var passwordService = scope.ServiceProvider.GetRequiredService<PasswordService>();

    await DbSeeder.SeedAsync(context, passwordService);
}

app.Run();

