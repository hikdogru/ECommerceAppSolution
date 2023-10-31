using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ECommerceApp.Core.DTOs;

namespace ECommerceApp.CatalogService.Models;

public class CategoryListCacheModel
{
    public IEnumerable<CategoryDTO> Data { get; set; }
    public int? TotalItems { get; set; }
    public int Page { get; set; }
}
