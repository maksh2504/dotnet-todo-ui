using System.Net.Http.Headers;
using System.Text.Json;
using System.Text;
using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using TodoApp.Client.Models;

namespace TodoApp.Client.Services;

public class AuthHttpHandler : DelegatingHandler
{
    private readonly IJSRuntime _js;
    private readonly NavigationManager _nav;

    public AuthHttpHandler(IJSRuntime js, NavigationManager nav)
    {
        _js = js;
        _nav = nav;
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var accessToken = await _js.InvokeAsync<string>("localStorage.getItem", "accessToken");
        if (!string.IsNullOrWhiteSpace(accessToken))
        {
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
        }

        var response = await base.SendAsync(request, cancellationToken);

        if (response.StatusCode == HttpStatusCode.Unauthorized)
        {
            var refreshToken = await _js.InvokeAsync<string>("localStorage.getItem", "refreshToken");

            if (!string.IsNullOrWhiteSpace(refreshToken))
            {
                var refreshRequest = new HttpRequestMessage(HttpMethod.Post, new Uri("https://localhost:7175/api/auth/refresh"))
                {
                    Content = new StringContent(
                        JsonSerializer.Serialize(new { refreshToken }),
                        Encoding.UTF8,
                        "application/json")
                };

                var refreshResponse = await base.SendAsync(refreshRequest, cancellationToken);

                if (refreshResponse.IsSuccessStatusCode)
                {
                    var tokens = await refreshResponse.Content.ReadFromJsonAsync<TokenResponseDto>(cancellationToken);

                    if (tokens is not null)
                    {
                        await _js.InvokeVoidAsync("localStorage.setItem", "accessToken", tokens.AccessToken);
                        await _js.InvokeVoidAsync("localStorage.setItem", "refreshToken", tokens.RefreshToken);

                        // retry original request with new token
                        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", tokens.AccessToken);
                        return await base.SendAsync(request, cancellationToken);
                    }
                }
            }

            _nav.NavigateTo("/login");
        }

        return response;
    }
}
