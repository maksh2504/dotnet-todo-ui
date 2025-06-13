using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.Net.Http.Json;
using TodoApp.Client.Models;

namespace TodoApp.Client.Services;

public class AuthService(HttpClient http, IJSRuntime js, NavigationManager nav) : IAuthService
{
    public async Task<bool> LoginAsync(LoginDto model)
    {
        var response = await http.PostAsJsonAsync("api/auth/login", model);
        if (!response.IsSuccessStatusCode)
            return false;

        var tokens = await response.Content.ReadFromJsonAsync<TokenResponseDto>();
        if (tokens is null)
            return false;

        await js.InvokeVoidAsync("localStorage.setItem", "accessToken", tokens.AccessToken);
        await js.InvokeVoidAsync("localStorage.setItem", "refreshToken", tokens.RefreshToken);
        nav.NavigateTo("/todos");
        return true;
    }

    public async Task<bool> RegisterAsync(LoginDto model)
    {
        var response = await http.PostAsJsonAsync("api/auth/register", model);
        if (!response.IsSuccessStatusCode)
            return false;

        nav.NavigateTo("/login");
        return true;
    }

    public async Task LogoutAsync()
    {
        await js.InvokeVoidAsync("localStorage.clear");
        nav.NavigateTo("/login");
    }
}