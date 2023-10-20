namespace ECommerceApp.AuthService.Models.User;

public class RegistrationRequest
{
    public string Email { get; set; }
    public string Password { get; set; }
    public string? PhoneNumber { get; set; }
}