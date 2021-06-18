using BlazorStatic.Shared.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Blazor.Api
{
    public class CurrentUserInfo
    {
        readonly UserManager<ApplicationUser> userManager;
        readonly SignInManager<ApplicationUser> signInManager;

        public CurrentUserInfo(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
        }

        [Function("CurrentUserInfo")]
        public async Task<HttpResponseData> Run([HttpTrigger(AuthorizationLevel.Function, "get")] HttpRequestData req)
        {           
            var response = req.CreateResponse(HttpStatusCode.OK);

            var name = req.Identities.FirstOrDefault()?.Name;

            return response;
        }
    }
}
