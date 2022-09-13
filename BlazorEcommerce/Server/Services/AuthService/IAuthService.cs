namespace BlazorEcommerce.Server.Services.AuthService;

public interface IAuthService
{
    Task<MessageResponse<int>> Register(User user, string password);

    Task<bool> UserExists(string email);

    Task<MessageResponse<string>> Login(string email, string password);

    Task<MessageResponse<bool>> ChangePassword(int userId, string newPassword);
}