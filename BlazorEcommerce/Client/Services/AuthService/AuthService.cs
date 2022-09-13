using BlazorEcommerce.Shared;
using System.Net.Http.Json;

namespace BlazorEcommerce.Client.Services.AuthService;

public class AuthService : IAuthService
{
    private readonly HttpClient _httpClient;

    public AuthService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<MessageResponse<int>> Register(UserRegister register)
    {
        var result = await _httpClient.PostAsJsonAsync("api/auth/register", register);
        return await result?.Content?.ReadFromJsonAsync<MessageResponse<int>>();
    }

    public async Task<MessageResponse<string>> Login(UserLogin userLogin)
    {
        var result = await _httpClient.PostAsJsonAsync("api/auth/login", userLogin);
        return await result?.Content?.ReadFromJsonAsync<MessageResponse<string>>();
    }

    public async Task<MessageResponse<bool>> ChangePassword(UserChangePassword password)
    {
        var response = await _httpClient.PostAsJsonAsync("api/auth/change-password", password.Password);
        return await response.Content.ReadFromJsonAsync<MessageResponse<bool>>();
    }
}