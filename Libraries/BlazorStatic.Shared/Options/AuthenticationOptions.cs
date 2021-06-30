namespace BlazorStatic.Shared
{
    public class AuthenticationOptions
    {
        public AuthenticationOptions()
        { }

        public string BaseUrl { get; set; }
        public string Authority { get; set; }
        public string DiscoveryEndPoint { get; set; }
        public string TokenEndPoint { get; set; }
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string Scope { get; set; }
    }
}
