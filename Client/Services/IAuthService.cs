using Microsoft.AspNetCore.Components.Authorization;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Security.Claims;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace BlazorStatic.Services
{
    public interface IAuthService
    {
        Task Login(LoginRequest loginRequest);
        Task Register(RegisterRequest registerRequest);
        Task Logout();
        Task<CurrentUser> CurrentUserInfo();
    }

    public class AuthService : IAuthService
    {
        private readonly HttpClient _httpClient;
        public AuthService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<CurrentUser> CurrentUserInfo()
        {
            var result = await _httpClient.GetFromJsonAsync<CurrentUser>("http://localhost:7071/api/CurrentUserInfo");
            return result;
        }
        public async Task Login(LoginRequest loginRequest)
        {           
            try
            {
                var result = await _httpClient.PostAsync("http://localhost:7071/api/Login", JsonContent.Create(loginRequest));

                if (result.StatusCode == System.Net.HttpStatusCode.BadRequest)
                    throw new Exception(await result.Content.ReadAsStringAsync());

                result.EnsureSuccessStatusCode();
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        public async Task Logout()
        {
            var result = await _httpClient.PostAsync("api/Logout", null);
            result.EnsureSuccessStatusCode();
        }
        public async Task Register(RegisterRequest registerRequest)
        {
            var result = await _httpClient.PostAsJsonAsync("http://localhost:7071/api/Register", registerRequest);
            if (result.StatusCode == System.Net.HttpStatusCode.BadRequest) throw new Exception(await result.Content.ReadAsStringAsync());
            result.EnsureSuccessStatusCode();
        }
    }

    public class CustomStateProvider : AuthenticationStateProvider
    {
        private readonly IAuthService api;
        private CurrentUser _currentUser;

        public CustomStateProvider(IAuthService api)
        {
            this.api = api;
        }
        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var identity = new ClaimsIdentity();
            try
            {
                var userInfo = await GetCurrentUser();
                if (userInfo.IsAuthenticated)
                {
                    var claims = new[] { new Claim(ClaimTypes.Name, _currentUser.UserName) }.Concat(_currentUser.Claims.Select(c => new Claim(c.Key, c.Value)));
                    identity = new ClaimsIdentity(claims, "Server authentication");
                }
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine("Request failed:" + ex.ToString());
            }
            return new AuthenticationState(new ClaimsPrincipal(identity));
        }
        private async Task<CurrentUser> GetCurrentUser()
        {
            if (_currentUser != null && _currentUser.IsAuthenticated) return _currentUser;
            _currentUser = await api.CurrentUserInfo();
            return _currentUser;
        }
        public async Task Logout()
        {
            await api.Logout();
            _currentUser = null;
            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        }
        public async Task Login(LoginRequest loginParameters)
        {
            await api.Login(loginParameters);
            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        }
        public async Task Register(RegisterRequest registerParameters)
        {
            await api.Register(registerParameters);
            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        }
    }

    public class RegisterRequest
    {
        [Required]
        [JsonPropertyName("userName")]
        public string UserName { get; set; }

        [Required]
        [JsonPropertyName("password")]
        public string Password { get; set; }

        [Required]
        [JsonPropertyName("passwordConfirm")]
        [Compare(nameof(Password), ErrorMessage = "Passwords do not match!")]
        public string PasswordConfirm { get; set; }
    }

    public class LoginRequest
    {
        [Required]
        [JsonPropertyName("userName")]
        public string UserName { get; set; }

        [Required]
        [JsonPropertyName("password")]
        public string Password { get; set; }

        [JsonPropertyName("rememberMe")]
        public bool RememberMe { get; set; }
    }

    public class CurrentUser
    {
        [JsonPropertyName("isAuthenticated")]
        public bool IsAuthenticated { get; set; }

        [JsonPropertyName("userName")]
        public string UserName { get; set; }

        [JsonPropertyName("claims")]
        public Dictionary<string, string> Claims { get; set; } = new Dictionary<string, string>();
    }
}
