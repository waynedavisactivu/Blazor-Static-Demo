using BlazorStatic.Services;
using BlazorStatic.Shared;
using BlazorStatic.Shared.Models;
using BlazorStatic.Shared.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.IO;
using System.Net;
using System.Text;
using System.Text.Json;
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

        [Function("Authenticate")]
        public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "get")] HttpRequestData req, FunctionContext executionContext)
        {
            var logger = executionContext.GetLogger("Authenticate");
            logger.LogInformation("C# HTTP trigger function processed a request.");

            var response = req.CreateResponse(HttpStatusCode.OK);
            response.Headers.Add("Content-Type", "text/plain; charset=utf-8");

            var tokenResponse = await Provider.GetAuthenticationToken(new BearerTokenRequest(Options));

            if (tokenResponse != null)
                response.Body = new MemoryStream(Encoding.UTF8.GetBytes(JsonSerializer.Serialize(tokenResponse)));            

            response.WriteString("Welcome to Azure Functions!");

            return new OkObjectResult(tokenResponse);
        }
    }
}
