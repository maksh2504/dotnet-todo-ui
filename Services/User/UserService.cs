using System.Net.Http.Json;
using TodoApp.Client.Models;

namespace TodoApp.Client.Services;

public class UserService(IHttpClientFactory httpClientFactory): IUserService
{
    public async Task<UserDto?> GetCurrentUserAsync()
    {
        var client = httpClientFactory.CreateClient("AuthorizedClient");
        var response = await client.GetAsync("api/users/current");
        if (!response.IsSuccessStatusCode)
            return null;

        return await response.Content.ReadFromJsonAsync<UserDto>();
    }
}