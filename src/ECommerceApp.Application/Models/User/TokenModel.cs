namespace ECommerceApp.Application.Models.User;

public class TokenModel
{
    public string Token { get; set; }
    public DateTime Expiration { get; set; }
}
