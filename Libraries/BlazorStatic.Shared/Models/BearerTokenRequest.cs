using IdentityModel.Client;
using System;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace BlazorStatic.Shared.Models
{
    public class BearerTokenRequest
    {
        public BearerTokenRequest() { }

        public BearerTokenRequest(AuthenticationOptions Option)
        {
            ClientSecret = Option.ClientSecret ?? throw new ArgumentNullException($"{nameof(ClientSecret)} cannot be null or empty");
            ClientId = Option.ClientId ?? throw new ArgumentNullException($"{nameof(ClientId)} cannot be null or empty");
            TokenEndpoint = Option.TokenEndPoint ?? throw new ArgumentNullException($"{nameof(TokenEndpoint)} cannot be null or empty");
            Scope = Option.Scope ?? throw new ArgumentNullException($"{nameof(Scope)} cannot be null or empty");

            RequestToken = new ClientCredentialsTokenRequest()
            {
                ClientSecret = ClientSecret,
                ClientId = ClientId,
                Address = TokenEndpoint,
                Scope = Scope
            };
        }
        public BearerTokenRequest(string ClientId, string ClientSecret, string Scope, string TokenEndpoint)
        {
            this.ClientSecret = ClientSecret ?? throw new ArgumentNullException($"{nameof(ClientSecret)} cannot be null or empty");
            this.ClientId = ClientId ?? throw new ArgumentNullException($"{nameof(ClientId)} cannot be null or empty");
            this.TokenEndpoint = TokenEndpoint ?? throw new ArgumentNullException($"{nameof(TokenEndpoint)} cannot be null or empty");
            this.Scope = Scope ?? throw new ArgumentNullException($"{nameof(Scope)} cannot be null or empty");

            RequestToken = new ClientCredentialsTokenRequest()
            {
                ClientSecret = ClientSecret,
                ClientId = ClientId,
                Address = TokenEndpoint,
                Scope = Scope
            };
        }

        [JsonPropertyName("clientId")]
        public string ClientId { get; set; }

        [JsonPropertyName("clientSecret")]
        public string ClientSecret { get; set; }

        [JsonPropertyName("scope")]
        public string Scope { get; set; }

        [JsonPropertyName("tokenEndpoint")]
        public string TokenEndpoint { get; set; }

        [IgnoreDataMember]
        public ClientCredentialsTokenRequest RequestToken { get; }
    }
}
