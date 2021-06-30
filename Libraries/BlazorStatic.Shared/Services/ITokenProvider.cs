using BlazorStatic.Shared.Models;
using IdentityModel.Client;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace BlazorStatic.Shared.Services
{
    public interface ITokenProvider
    {
        Task<BearerTokenResponse> GetAuthenticationToken(BearerTokenRequest Request);
    }

    public class TokenProviderClient : ITokenProvider
    {
        private HttpClient Client;
        private BearerTokenRequest Request;        

        public TokenProviderClient(HttpClient Client)
        {
            this.Client = Client;

        }
        public async Task<BearerTokenResponse> GetAuthenticationToken(BearerTokenRequest Request)
        {
            this.Request = Request;

            var token = await TokenFactory();

            return token;
        }

        private async Task<BearerTokenResponse> TokenFactory()
        {
            return await Client.RequestClientCredentialsTokenAsync(Request.RequestToken, new CancellationToken());
        }
    }
}
