using CodeVsk.Dotnet.Authentication.Application.Services.Auth.Interfaces;
using CodeVsk.Dotnet.Authentication.Application.Services.Authentication;
using CodeVsk.Dotnet.Authentication.Infra.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace CodeVsk.Dotnet.Authentication.Api.IoC
{
    public static class NativeInjectorConfig
    {
        public static void RegisterServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<IdentityDataContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"))
            );

            services.AddDefaultIdentity<IdentityUser>()
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<IdentityDataContext>()
                .AddDefaultTokenProviders();

            services.AddScoped<IAuthService, AuthService>();
        }
    }
}
