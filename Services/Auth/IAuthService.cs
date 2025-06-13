using TodoApp.Client.Models;

namespace TodoApp.Client.Services;

public interface IAuthService
{
    Task<bool> LoginAsync(LoginDto model);
    Task<bool> RegisterAsync(LoginDto model);
    Task LogoutAsync();
}
