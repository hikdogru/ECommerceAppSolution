using Microsoft.AspNetCore.Mvc;
using ECommerceApp.WebApiAbstraction.Models;


namespace ECommerceApp.WebApiAbstraction.Controllers;

public abstract class ApiControllerBase : ControllerBase
{
    protected IActionResult CreateActionResult<T>(T data, int statusCode, Exception? exception = null)
    {
        var response = new ResponseModel<T>
        {
            StatusCode = statusCode,
            Data = data,
            Status = IsSuccessStatusCode(statusCode),
            IsSuccessful = IsSuccessStatusCode(statusCode)
        };

        if (exception != null)
        {
            response.Errors.AddRange(GetExceptionMessages(exception));
        }

        return new ObjectResult(response)
        {
            StatusCode = response.StatusCode,
        };
    }

    private static bool IsSuccessStatusCode(int statusCode)
    {
        return statusCode >= 200 && statusCode < 300;
    }

    private static IEnumerable<string> GetExceptionMessages(Exception exception)
    {
        var messages = new List<string>();
        while (exception != null)
        {
            messages.Add(exception.Message);
            exception = exception.InnerException;
        }
        return messages;
    }
}
