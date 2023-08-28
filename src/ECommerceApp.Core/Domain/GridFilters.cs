using Newtonsoft.Json;

namespace ECommerceApp.Core.Domain;

public class GridFilters
{
    [JsonProperty("logic")]
    public string Logic { get; set; }

    [JsonProperty("filters")]
    public List<GridFilterModel> Filters { get; set; }
}