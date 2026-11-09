using Microsoft.EntityFrameworkCore;
using Restaurante.Web.Data;
using Restaurante.Web.Services.Abstractions;
using Restaurante.Web.Services.Implementations;

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

            return builder;
        }

        private static void AddServices(WebApplicationBuilder builder)
        {
            builder.Services.AddScoped<ICategoriesService, CategoriesService>();
            builder.Services.AddScoped<IDishesService, DishesService>();
            builder.Services.AddScoped<IRolesService, RolesService>();
            builder.Services.AddScoped<IEmployeesService, EmployeesService>();
            builder.Services.AddScoped<ITablesService, TablesService>();
            builder.Services.AddScoped<IOrdersService, OrdersService>();
        }
    }
}