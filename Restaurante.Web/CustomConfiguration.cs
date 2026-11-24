using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Restaurante.Web.Core;
using Restaurante.Web.Data;
using Restaurante.Web.Data.Entities;
using Restaurante.Web.Data.Seeders;
using Restaurante.Web.Helpers.Abstractions;
using Restaurante.Web.Helpers.Implementations;
using Restaurante.Web.Services.Abstractions;
using Restaurante.Web.Services.Implementations;


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

            string jwtIssuer = builder.Configuration["JWT:Issuer"]
                ?? throw new InvalidOperationException("Falta JWT:Issuer en user-secrets");
            string jwtAudience = builder.Configuration["JWT:Audience"]
                ?? throw new InvalidOperationException("Falta JWT:Audience en user-secrets");
            string jwtSignKey = builder.Configuration["JWT:SignKey"]
                ?? throw new InvalidOperationException("Falta JWT:SignKey en user-secrets");

            const string CookieOrJwt = "CookieOrJwt";

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = CookieOrJwt;
                options.DefaultChallengeScheme = CookieOrJwt;
            })
            .AddPolicyScheme(CookieOrJwt, "Cookie for pages, JWT for /api", options =>
            {
                options.ForwardDefaultSelector = context =>
                    context.Request.Path.StartsWithSegments("/api")
                        ? JwtBearerDefaults.AuthenticationScheme
                        : IdentityConstants.ApplicationScheme;
            })
            .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,

                    ValidIssuer = jwtIssuer,
                    ValidAudience = jwtAudience,
                    IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(jwtSignKey)),

                    ClockSkew = TimeSpan.Zero
                };
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