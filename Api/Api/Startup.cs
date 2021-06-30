using BlazorStatic.Shared;
using BlazorStatic.Shared.Services;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net.Http;

[assembly: FunctionsStartup(typeof(Api.Startup))]

namespace Api
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services.AddHttpClient("TenantClient", c =>
            {
                c.BaseAddress = new Uri("https://visi-cloud-tenantservice.azurewebsites.net");
            });

            builder.Services.AddHttpClient("UserClient", c =>
            {
                c.BaseAddress = new Uri("https://visi-cloud-user.azurewebsites.net");
            });

            builder.Services.AddHttpClient("AuthenticateClient", c =>
            {
                c.BaseAddress = new Uri("https://visi-cloud-authentication.azurewebsites.net");
            });

            builder.Services.AddScoped<IAuthenticatedUsers, AuthenticatedUsers>();

            builder.Services.AddScoped<ITokenProvider>(sp =>
            {
                var client = sp.GetRequiredService<HttpClient>();

                return new TokenProviderClient(client);
            });
            builder.Services.Configure<AuthenticationOptions>(options =>
            {
                options.BaseUrl = "https://visi-cloud-authentication.azurewebsites.net";
                options.Authority = "https://visi-cloud-authentication.azurewebsites.net";

                options.Scope = "tenant-api";
                options.ClientId = "activu-member";
                options.ClientSecret = "secret";
                options.TokenEndPoint = $"https://visi-cloud-authentication.azurewebsites.net/connect/token";
                options.DiscoveryEndPoint = $"https://visi-cloud-authentication.azurewebsites.net/.well-known/openid-configuration";
            });
        }
    }
}
