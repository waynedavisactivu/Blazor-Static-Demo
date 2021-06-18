using BlazorStatic.Shared.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using System.Net;
using System.Threading.Tasks;

namespace Blazor.Api
{
    public class Logout
    {
        readonly SignInManager<ApplicationUser> signInManager;

        public Logout(SignInManager<ApplicationUser> signInManager)
        {
            this.signInManager = signInManager;
        }

        [Function("Logout")]
        public async Task<HttpResponseData> Run([HttpTrigger(AuthorizationLevel.Function, "post")] HttpRequestData req, FunctionContext executionContext)
        {
            await signInManager.SignOutAsync();

            var response = req.CreateResponse(HttpStatusCode.OK);

            return response;
        }
    }
}
