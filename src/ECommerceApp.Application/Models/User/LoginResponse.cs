namespace ECommerceApp.Application.Models.User;

public class LoginResponse
{
    public string Id { get; set; }
    public string Email { get; set; }
    public string? PhoneNumber { get; set; }
    public TokenModel TokenModel { get; set; }
}
