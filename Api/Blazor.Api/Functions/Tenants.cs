using BlazorStatic.Shared;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;

namespace Blazor.Api.Functions
{
    public class Tenants
    {
        readonly HttpClient Client;

        public Tenants(IHttpClientFactory Factory)
        {
            Client = Factory.CreateClient("TenantClient");
        }

        [Function("Tenants")]
        public async Task<HttpResponseData> Run([HttpTrigger(AuthorizationLevel.Function, "get")] HttpRequestData req, FunctionContext executionContext)
        {
            var logger = executionContext.GetLogger("Tenants");
            logger.LogInformation("C# HTTP trigger function processed a request.");

            var response = req.CreateResponse(HttpStatusCode.OK);
            response.Headers.Add("Content-Type", "text/plain; charset=utf-8");

            var tenants = await Client.GetFromJsonAsync<Tenant>("api/tenant/get");

            if (tenants != null)
            {
                var data = JsonSerializer.SerializeToUtf8Bytes(tenants);

                response.Body = new MemoryStream(data);
            }

            response.WriteString("Welcome to Azure Functions!");

            return response;
        }
    }
}
