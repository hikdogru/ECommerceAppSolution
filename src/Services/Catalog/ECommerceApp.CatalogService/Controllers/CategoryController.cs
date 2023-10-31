using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Asp.Versioning;
using ECommerceApp.Application.Services.Abstract;
using ECommerceApp.CatalogService.Models;
using ECommerceApp.Core.Domain;
using ECommerceApp.Core.DTOs;
using ECommerceApp.Infrastructure.Redis;
using ECommerceApp.WebApiAbstraction.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace ECommerceApp.CatalogService.Controllers
{
    [ApiVersion("1.0")]
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class CategoryController : ApiControllerBase
    {
        #region Fields

        private readonly ICategoryService _categoryService;
        private readonly IRedisContext _redisContext;


        #endregion

        #region Ctor


        public CategoryController(ICategoryService categoryService, IRedisContext redisContext)
        {
            _categoryService = categoryService;
            _redisContext = redisContext;
        }


        #endregion


        #region Methods


        [Route("Get")]
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] int page = 1, [FromQuery] int size = 10, [FromBody] GridFilters? filter = null)
        {
            string cacheKey = "categories";
            var cachedData = await _redisContext.GetObject<CategoryListCacheModel>(cacheKey);
            if (cachedData is not null && filter is null && cachedData.Page == page)
                return CreateActionResult<object>(cachedData.Data, statusCode: StatusCodes.Status200OK, totalCounts: cachedData.TotalItems);


            var categories = await _categoryService.Filter(page, size, filter);
            if (filter is not null)
            {
                await _redisContext.Remove(cacheKey);
                return CreateActionResult<object>(categories, statusCode: StatusCodes.Status200OK, totalCounts: categories.TotalItems);
            }

            var cacheModel = new CategoryListCacheModel()
            {
                Data = categories,
                TotalItems = categories.TotalItems,
                Page = page
            };
            _redisContext.SaveObject(cacheKey, cacheModel);
            return CreateActionResult<object>(categories, statusCode: StatusCodes.Status200OK, totalCounts: categories.TotalItems);
        }

        #endregion
    }
}