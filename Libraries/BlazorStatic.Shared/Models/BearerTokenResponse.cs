using IdentityModel.Client;
using System.Net;
using System.Text.Json.Serialization;

namespace BlazorStatic.Shared.Models
{
    public class BearerTokenResponse
    {
        public BearerTokenResponse() { }

        private BearerTokenResponse(TokenResponse r)
        {
            Response = r;
            IsError = r.IsError;
            AccessToken = r.AccessToken;

            ErrorDescription = r.HttpErrorReason;
            HttpErrorReason = r.HttpErrorReason;
            HttpStatusCode = r.HttpStatusCode;
        }

        [JsonPropertyName("isError")]
        public bool IsError { get; set; }

        [JsonPropertyName("accessToken")]
        public string AccessToken { get; set; }

        [JsonIgnore]
        public TokenResponse Response { get; set; }

        [JsonPropertyName("errorDescription")]
        public string ErrorDescription { get; set; }

        [JsonPropertyName("httpErrorReason")]
        public string HttpErrorReason { get; set; }

        [JsonPropertyName("httpStatusCode")]
        public HttpStatusCode HttpStatusCode { get; set; }

        public static implicit operator BearerTokenResponse(TokenResponse r) => new BearerTokenResponse(r);
    }
}
