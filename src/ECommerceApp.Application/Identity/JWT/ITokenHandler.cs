using ECommerceApp.Application.Models.User;
using ECommerceApp.Core.Domain.Entities.Identity;

namespace ECommerceApp.Application.Identity.JWT;

public interface ITokenHandler
{
    TokenModel GenerateToken(AppUser user, int minute, IEnumerable<string> roles);
}