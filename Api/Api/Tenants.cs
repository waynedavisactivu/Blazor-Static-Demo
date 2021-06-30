using BlazorStatic.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
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

        [FunctionName("Tenants")]
        public async Task<IEnumerable<Tenant>> Run([HttpTrigger(AuthorizationLevel.Function, "get")] HttpRequest req)
        {            
            var tenants = await Client.GetFromJsonAsync<IEnumerable<Tenant>>("api/tenant/get");            

            return tenants;
        }
    }
}
