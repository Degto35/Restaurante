using Microsoft.EntityFrameworkCore;
using Restaurante.Web.Data;
using Restaurante.Web.Services.Abstractions;
using Restaurante.Web.Services.Implementations;
using Restaurante.Web.Helpers.Abstractions;
using Restaurante.Web.Helpers.Implementations;
using Restaurante.Web.Data.Seeders;

namespace Restaurante.Web
{
    public static class CustomConfiguration
    {
        public static WebApplicationBuilder AddCustomConfiguration(this WebApplicationBuilder builder)
        {
            // Data Context
            builder.Services.AddDbContext<DataContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("MyConnection"));
            });

            // AutoMapper
            builder.Services.AddAutoMapper(typeof(Program));

            // Services
            AddServices(builder);
            builder.Services.AddTransient<SeedDb>();

            return builder;
        }

        private static void AddServices(WebApplicationBuilder builder)
        {
            builder.Services.AddScoped<ICategoriesService, CategoriesService>();
            builder.Services.AddScoped<IDishesService, DishesService>();
            builder.Services.AddScoped<IRolesService, RolesService>();
            builder.Services.AddScoped<IEmployeesService, EmployeesService>();
            builder.Services.AddScoped<IDiningTablesService, DiningTablesService>();
            builder.Services.AddScoped<IOrdersService, OrdersService>();
            builder.Services.AddScoped<ISelectListHelper, SelectListHelper>();



        }
        public static WebApplication AddCustomWebApplicationConfiguration(this WebApplication app)
        {
            SeedData(app);

            return app;
        }

        private static void SeedData(WebApplication app)
        {
            IServiceScopeFactory scopeFactory = app.Services.GetRequiredService<IServiceScopeFactory>();

            using IServiceScope scope = scopeFactory.CreateScope();
            SeedDb service = scope.ServiceProvider.GetRequiredService<SeedDb>();
            service.SeedAsync().Wait();
        }
    }

}