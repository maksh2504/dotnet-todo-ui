namespace TodoApp.Client.Models
{
    public class LoginDto
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
    
    public class RefreshTokenRequestDto
    {
        public Guid UserId { get; set; }
        public required string RefreshToken { get; set; }
    }
    
    public class TokenResponseDto
    {
        public required string AccessToken { get; set; }
        public required string RefreshToken { get; set; }
    }
}