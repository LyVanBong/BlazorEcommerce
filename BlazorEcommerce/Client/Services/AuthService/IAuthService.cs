using BlazorEcommerce.Shared;

namespace BlazorEcommerce.Client.Services.AuthService;

public interface IAuthService
{
    Task<MessageResponse<int>> Register(UserRegister register);

    Task<MessageResponse<string>> Login(UserLogin userLogin);

    Task<MessageResponse<bool>> ChangePassword(UserChangePassword password);
}