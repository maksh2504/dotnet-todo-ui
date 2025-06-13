using TodoApp.Client.Models;

namespace TodoApp.Client.Services;

public interface ITodoService
{
    Task<List<TodoDto>?> GetTodosAsync(string sort, string? finished);
    Task<TodoDto?> CreateTodoAsync(CreateTodoDto dto);
    Task<TodoDto?> UpdateTodoAsync(int id, object patch);
    Task<bool> DeleteTodoAsync(int id);
}
