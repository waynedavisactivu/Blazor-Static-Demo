using BlazorStatic.Shared.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Security.Claims;
using System.Text.Json;
using System.Threading.Tasks;

namespace Blazor.Api
{
    public class Login
    {
        readonly UserManager<ApplicationUser> userManager;
        readonly SignInManager<ApplicationUser> signInManager;

        public Login(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
        }

        [Function("Login")]
        public async Task<HttpResponseData> Run([HttpTrigger(AuthorizationLevel.Function, "post")] HttpRequestData req,  FunctionContext executionContext)
        {
            var streamData = new StreamReader(req.Body);

            var data = JsonSerializer.Deserialize<UserRequest>(await streamData.ReadToEndAsync());

            string userName = data.Username;
            string password = data.Password;

            bool result = false;

            var appUser = await userManager.FindByEmailAsync(userName);

            if (appUser != null)
            {
                result = await userManager.CheckPasswordAsync(appUser, password);

                var response = req.CreateResponse(HttpStatusCode.OK);
                var userClaims = await userManager.GetClaimsAsync(appUser);
                var claims = new Dictionary<string, string>();

                foreach (var item in userClaims)
                    claims.TryAdd(item.Type, item.Value);

                var user = new CurrentUser
                {
                    IsAuthenticated = result,
                    UserName = userName,
                    Claims = claims
                };

                if (result)
                {
                    var identity = ClaimsPrincipal.Current.Identity;

                    await signInManager.SignInAsync(appUser, true);
                }

                var dataBytes = JsonSerializer.SerializeToUtf8Bytes(user);
                response.Body = new MemoryStream(dataBytes);

                return response;
            }

            return req.CreateResponse(HttpStatusCode.BadRequest);
        }
    }
}
