using Newtonsoft.Json;


namespace ECommerceApp.Core.Domain;

public class GridFilterModel
{
    [JsonProperty("field")]
    public string Field { get; set; }

    [JsonProperty("operator")]
    public string Operator { get; set; }

    [JsonProperty("value")]
    public string Value { get; set; }
}