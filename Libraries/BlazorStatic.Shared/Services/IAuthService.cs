using BlazorStatic.Shared;
using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace BlazorStatic.Shared.Services
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
}
