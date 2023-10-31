using System.Net;
using System.Text;
using ECommerceApp.WebUI.Models;
using Newtonsoft.Json;

namespace ECommerceApp.WebUI.Services;

public class ApiBaseService : IApiBaseService
{
    private readonly IHttpClientFactory _httpClientFactory;

    public ApiBaseService(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public async Task<ApiResponseModel> SendAsync(ApiRequestModel model)
    {
        try
        {
            var client = _httpClientFactory.CreateClient("ECommerceAPI");
            var message = new HttpRequestMessage();
            message.Headers.Add("Accept", "application/json");

            message.RequestUri = new Uri(model.Url);
            if (model.Data is not null)
            {
                message.Content = new StringContent(
                    JsonConvert.SerializeObject(model.Data), Encoding.UTF8, "application/json");
            }

            var apiResponse = await client.SendAsync(message);
            switch (apiResponse.StatusCode)
            {
                case HttpStatusCode.OK:
                    var responseContent = await apiResponse.Content.ReadAsStringAsync();
                    var responseObject = JsonConvert.DeserializeObject<ApiResponseModel>(responseContent);
                    return new ApiResponseModel
                    {
                        Status = true,
                        Data = responseObject.Data,
                        TotalCounts = responseObject.TotalCounts
                    };
                case HttpStatusCode.BadRequest:
                    return new ApiResponseModel
                    {
                        Status = false,
                        Message = "Bad request"
                    };
                case HttpStatusCode.Unauthorized:
                    return new ApiResponseModel
                    {
                        Status = false,
                        Message = "Unauthorized"
                    };
                case HttpStatusCode.NotFound:
                    return new ApiResponseModel
                    {
                        Status = false,
                        Message = "Not found"
                    };
                default:
                    return new ApiResponseModel
                    {
                        Status = false,
                        Message = "Unknown error"
                    };
            }

        }
        catch (Exception ex)
        {
            return new ApiResponseModel
            {
                Message = ex.Message,
                Status = false
            };
        }
    }
}
