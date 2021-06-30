using BlazorStatic.Shared;
using BlazorStatic.Shared.Models;
using BlazorStatic.Shared.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;

namespace Blazor.Api.Functions
{
    public class Authenticate
    {
        readonly ITokenProvider Provider;
        readonly AuthenticationOptions Options;

        public Authenticate(
            ITokenProvider Provider,
            IOptions<AuthenticationOptions> Options)
        {
            this.Provider = Provider;
            this.Options = Options.Value;
        }

        [FunctionName("Authenticate")]
        public async Task<BearerTokenResponse> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "authenticate")] HttpRequest req)
        {            
            var tokenResponse = await Provider.GetAuthenticationToken(new BearerTokenRequest(Options));            

            return tokenResponse;
        }
    }
}
