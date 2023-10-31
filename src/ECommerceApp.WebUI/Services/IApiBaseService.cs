using ECommerceApp.WebUI.Models;

namespace ECommerceApp.WebUI.Services;

public interface IApiBaseService
{
    Task<ApiResponseModel> SendAsync(ApiRequestModel model);
}
