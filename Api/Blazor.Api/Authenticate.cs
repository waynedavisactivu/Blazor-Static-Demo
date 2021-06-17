using BlazorStatic.Shared.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using System.IO;
using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Blazor.Api
{
    public class Authenticate
    {        
        readonly UserManager<ApplicationUser> userManager;

        public Authenticate(UserManager<ApplicationUser> userManager)
        {
            this.userManager = userManager;
        }

        [Function("Authenticate")]
        public async Task<HttpResponseData> Run([HttpTrigger(AuthorizationLevel.Function, "get", Route = null)] HttpRequestData req)
        {
            var streamData = new StreamReader(req.Body);

            var data = JsonSerializer.Deserialize<UserRequest>(await streamData.ReadToEndAsync());

            string userName = data.Username;
            string password = data.Password;

            bool result = false;

            var appUser = await userManager.FindByEmailAsync(userName);

            if (appUser != null)
                result = await userManager.CheckPasswordAsync(appUser, password);            

            var resultString = result ? "Success" : "Failed";

            string responseMessage = string.IsNullOrEmpty(userName)
                ? "This HTTP triggered function executed successfully. Pass a name in the query string or in the request body for a personalized response."
                : $"Hello, {userName}. This HTTP triggered function executed successfully. Authentication Result: { resultString }.";

            return req.CreateResponse(HttpStatusCode.OK);
        }
    }

    public class UserRequest
    {
        [JsonPropertyName("username")]
        public string Username { get; set; }

        [JsonPropertyName("password")]
        public string Password { get; set; }
    }
}
