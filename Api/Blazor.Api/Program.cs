using BlazorStatic.Shared;
using BlazorStatic.Shared.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Blazor.Api
{
    public class Program
    {
        public static void Main()
        {
            var host = new HostBuilder()
                .ConfigureFunctionsWorkerDefaults()
                .ConfigureServices(services => 
                {
                    services.AddHttpClient();

                    services.AddDbContext<ApplicationDbContext>(o =>
                        o.UseSqlServer("Server=tcp:activu-sqlserver.database.windows.net,1433;Initial Catalog=Activu-UserDb;Persist Security Info=False;User ID=activuadmin;Password=V0lunteer$;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;"));

                    services.AddDefaultIdentity<ApplicationUser>(o =>
                    o.SignIn.RequireConfirmedAccount = true)
                        .AddEntityFrameworkStores<ApplicationDbContext>();

                    services.AddIdentityServer()
                        .AddApiAuthorization<ApplicationUser, ApplicationDbContext>();

                    services.AddAuthentication()
                        .AddIdentityServerJwt();
                })
                .Build();

            host.Run();
        }
    }
}