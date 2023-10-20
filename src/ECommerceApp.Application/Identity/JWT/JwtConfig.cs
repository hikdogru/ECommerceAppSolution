namespace ECommerceApp.Application.Identity.JWT;

public class JwtConfig
{
    public string SecurityKey { get; set; }
    public string Issuer { get; set; }
    public string Audience { get; set; }
}
