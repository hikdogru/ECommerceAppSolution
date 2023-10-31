namespace ECommerceApp.WebUI.Models;

public class ApiRequestModel
{
    public HttpMethod Methods { get; set; } = HttpMethod.Get;
    public string Url { get; set; }

    public object Data { get; set; }
    public string AccessToken { get; set; }
}
