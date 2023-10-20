using ECommerceApp.Application.Models.User;
using ECommerceApp.AuthService.Models.User;

namespace ECommerceApp.Application.Services.Abstract;
public interface IUserService
{
    Task<string?> RegisterAsync(RegistrationRequest registerModel);
    Task<LoginResponse> LoginAsync(LoginRequest loginRequest);
    Task<bool> AssignRole(string email, string roleName);
}
