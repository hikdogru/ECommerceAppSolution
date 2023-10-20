using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text.Json.Serialization;

namespace ECommerceApp.WebApiAbstraction.Models;
public class ResponseModel<T>
{
    public ResponseModel()
    {
        Errors = new List<string>();
    }
    public T Data { get; set; }
    [JsonIgnore]
    public int StatusCode { get; set; }
    [JsonIgnore]
    public bool IsSuccessful { get; set; }
    public bool Status { get; set; }
    public List<string> Errors { get; set; }
    public static ResponseModel<T> Success(T data, int statusCode)
    {
        return new ResponseModel<T> { StatusCode = statusCode, Data = data, IsSuccessful = true, Status = true };
    }
    public static ResponseModel<T> Success(int statusCode)
    {
        return new ResponseModel<T> { StatusCode = statusCode, Data = default(T), IsSuccessful = true, Status = true };
    }
    public static ResponseModel<T> Fail(List<string> errors, int statusCode)
    {
        return new ResponseModel<T> { Errors = errors, StatusCode = statusCode, IsSuccessful = false, Status = false };
    }
    public static ResponseModel<T> Fail(string error, int statusCode)
    {
        return new ResponseModel<T> { Errors = new List<string> { error }, StatusCode = statusCode, IsSuccessful = false, Status = false };
    }
}
