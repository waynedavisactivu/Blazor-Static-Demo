using System.Text.Json.Serialization;

namespace BlazorStatic.Shared
{
    public class Tenant
    {
        [JsonPropertyName("partitionKey")]
        public string PartitionKey { get; set; }

        [JsonPropertyName("rowKey")]
        public string RowKey { get; set; }

        [JsonPropertyName("clientId")]
        public string ClientId { get; set; }

        [JsonPropertyName("adminLogin")]
        public string AdminLogin { get; set; }

        [JsonPropertyName("adminPasswordHash")]
        public string AdminPasswordHash { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("domain")]
        public string Domain { get; set; }

        [JsonPropertyName("connectionString")]
        public string ConnectionString { get; set; }
    }
}
