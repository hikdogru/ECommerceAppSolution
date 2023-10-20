using ECommerceApp.Application.Models.User;
using ECommerceApp.Application.Services.Abstract;
using ECommerceApp.AuthService.Models.User;
using ECommerceApp.Core.Domain.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using ECommerceApp.Application.Identity.JWT;

namespace ECommerceApp.Application.Services.Concrete;
public class UserService : IUserService
{
    private readonly UserManager<AppUser> _userManager;
    private readonly ITokenHandler _tokenHandler;
    private readonly RoleManager<AppRole> _roleManager;

    public UserService(ITokenHandler tokenHandler, UserManager<AppUser> userManager, RoleManager<AppRole> roleManager)
    {
        _tokenHandler = tokenHandler;
        _userManager = userManager;
        _roleManager = roleManager;
    }

    public async Task<bool> AssignRole(string email, string roleName)
    {
        var user = await _userManager.FindByEmailAsync(email);
        if (user is AppUser)
        {
            var isRoleExists = await _roleManager.RoleExistsAsync(roleName);
            if (!isRoleExists)
            {
                await _roleManager.CreateAsync(new() { Name = roleName });
            }
            await _userManager.AddToRoleAsync(user, roleName);
            return true;
        }
        return false;
    }

    public async Task<LoginResponse> LoginAsync(LoginRequest loginRequest)
    {
        var user = await _userManager.FindByEmailAsync(loginRequest.Email);
        var isUserValid = user != null && await _userManager.CheckPasswordAsync(user, loginRequest.Password);
        if (isUserValid)
        {
            var roles = await _userManager.GetRolesAsync(user);
            var token = _tokenHandler.GenerateToken(user, 20, roles);
            return new()
            {
                Email = user.Email,
                Id = user.Id,
                PhoneNumber = user?.PhoneNumber ?? "",
                TokenModel = token
            };
        }

        return new()
        {
            TokenModel = null
        };
    }

    public async Task<string?> RegisterAsync(RegistrationRequest registerModel)
    {
        var user = new AppUser
        {
            UserName = registerModel.Email,
            Email = registerModel.Email,
            PhoneNumber = registerModel.PhoneNumber,
        };

        var result = await _userManager.CreateAsync(user, registerModel.Password);
        if (result.Succeeded)
        {
            var userInDb = await _userManager.FindByEmailAsync(user.Email);
            if (userInDb is not null)
            {
                return null;
            }

            throw new ArgumentNullException(nameof(userInDb));
        }
        return result.Errors.FirstOrDefault().Description;
    }
}
