using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace Blazor.Api
{
    public class Register
    {
        readonly HttpClient Client;

        public Register(IHttpClientFactory Factory)
        {
            Client = Factory.CreateClient("UserClient");
        }

        [Function("Register")]
        public async Task<HttpResponseData> Run([HttpTrigger(AuthorizationLevel.Function, "post")] HttpRequestData req, FunctionContext executionContext)
        {
            var response = req.CreateResponse(HttpStatusCode.OK);


            return response;
        }
    }
}
