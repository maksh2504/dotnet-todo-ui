using TodoApp.Client.Models;

namespace TodoApp.Client.Services;

public interface IUserService
{
    Task<UserDto?> GetCurrentUserAsync();
}
