using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ECommerceApp.WebUI.Models;

public class ApiResponseModel
{

    public string Message { get; set; }

    [JsonProperty("data")]
    public object? Data { get; set; }

    [JsonProperty("status")]
    public bool Status { get; set; }

    [JsonProperty("errors")]
    public object[] Errors { get; set; }

    [JsonProperty("totalCounts")]
    public int? TotalCounts { get; set; }

}
