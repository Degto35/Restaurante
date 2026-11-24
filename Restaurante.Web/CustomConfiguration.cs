using Microsoft.EntityFrameworkCore;
using Restaurante.Web.Data;
using Restaurante.Web.Services.Abstractions;
using Restaurante.Web.Services.Implementations;
using Restaurante.Web.Helpers.Abstractions;
using Restaurante.Web.Helpers.Implementations;
using Microsoft.AspNetCore.Identity;
using Restaurante.Web.Data.Entities;
using Restaurante.Web.Data.Seeders;
using Restaurante.Web.Core;

namespace Restaurante.Web
{
    public static class CustomConfiguration
    {
        public static WebApplicationBuilder AddCustomConfiguration(this WebApplicationBuilder builder)
        {
            
            builder.Services.AddDbContext<DataContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("MyConnection"));
            });

            
            builder.Services.AddAutoMapper(typeof(Program));

      
            AddServices(builder);
            AddIAM(builder);

            return builder;
        }

        private static void AddServices(WebApplicationBuilder builder)
        {
            builder.Services.AddScoped<ICategoriesService, CategoriesService>();
            builder.Services.AddScoped<IDishesService, DishesService>();
            builder.Services.AddScoped<IRolesService, RolesService>();
            builder.Services.AddHttpContextAccessor();
            builder.Services.AddScoped<IAccountService, AccountService>();
            builder.Services.AddScoped<IDiningTablesService, DiningTablesService>();
            builder.Services.AddScoped<IOrdersService, OrdersService>();
            builder.Services.AddScoped<ISelectListHelper, SelectListHelper>();
            builder.Services.AddTransient<SeedDb>();
            builder.Services.AddScoped<IUsersService, UsersService>();
            builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("Email"));
            builder.Services.AddScoped<IEmailService, EmailService>();
        }
        
        private static void AddIAM(WebApplicationBuilder builder)
        {
            builder.Services.AddIdentity<User, IdentityRole>(config =>
            {
                config.User.RequireUniqueEmail = true;

                
                config.Password.RequireDigit = false;
                config.Password.RequiredUniqueChars = 0;
                config.Password.RequireLowercase = false;
                config.Password.RequireUppercase = false;
                config.Password.RequireNonAlphanumeric = false;
                config.Password.RequiredLength = 4;
            }).AddEntityFrameworkStores<DataContext>()
              .AddDefaultTokenProviders();
        
            builder.Services.Configure<DataProtectionTokenProviderOptions>(options =>
            {
                options.TokenLifespan = TimeSpan.FromHours(1);
            });

            builder.Services.ConfigureApplicationCookie(config =>
            {
                config.Cookie.Name = "Auth";
                config.ExpireTimeSpan = TimeSpan.FromHours(8);   
                config.SlidingExpiration = true;                 
                config.LoginPath = "/Account/Login";             
                config.AccessDeniedPath = "/Account/AccessDenied";
            });
        }
        public static WebApplication AddCustomWebApplicationConfiguration(this WebApplication app)
        {
            SeedData(app);

            return app;
        }

        private static void SeedData(WebApplication app)
        {
            
            using IServiceScope scope = app.Services.CreateScope();
            SeedDb seeder = scope.ServiceProvider.GetRequiredService<SeedDb>();

            seeder.SeedAsync().GetAwaiter().GetResult();
        }
    }
}