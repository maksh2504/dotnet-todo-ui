using System.Net.Http.Json;
using TodoApp.Client.Models;

namespace TodoApp.Client.Services;

public class TodoService(IHttpClientFactory httpClientFactory) : ITodoService
{
    public async Task<List<TodoDto>?> GetTodosAsync(string sort, string? finished)
    {
        var client = httpClientFactory.CreateClient("AuthorizedClient");
        var query = $"?sort={sort}";

        if (!string.IsNullOrWhiteSpace(finished))
            query += $"&finished={finished}";

        var response = await client.GetAsync($"api/todos{query}");
        return await response.Content.ReadFromJsonAsync<List<TodoDto>>();
    }

    public async Task<TodoDto?> CreateTodoAsync(CreateTodoDto dto)
    {
        var client = httpClientFactory.CreateClient("AuthorizedClient");
        var response = await client.PostAsJsonAsync("api/todos", dto);
        return response.IsSuccessStatusCode
            ? await response.Content.ReadFromJsonAsync<TodoDto>()
            : null;
    }

    public async Task<TodoDto?> UpdateTodoAsync(int id, object patch)
    {
        var client = httpClientFactory.CreateClient("AuthorizedClient");
        var response = await client.PatchAsJsonAsync($"api/todos/{id}", patch);
        return response.IsSuccessStatusCode
            ? await response.Content.ReadFromJsonAsync<TodoDto>()
            : null;
    }

    public async Task<bool> DeleteTodoAsync(int id)
    {
        var client = httpClientFactory.CreateClient("AuthorizedClient");
        var response = await client.DeleteAsync($"api/todos/{id}");
        return response.IsSuccessStatusCode;
    }
}