using System.Text.Json;
using AutoMapper;
using ECommerceApp.Application.Services.Abstract;
using ECommerceApp.Core.Domain;
using ECommerceApp.Core.Domain.Entities;
using ECommerceApp.Core.DTOs;
using ECommerceApp.Core.Extensions;
using ECommerceApp.WebUI.Areas.Admin.Controllers;
using ECommerceApp.WebUI.Models;
using ECommerceApp.WebUI.Models.Category;
using ECommerceApp.WebUI.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver.Linq;
using Moq;
using NToastNotify;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace WebUI.Test
{
    public class CategoryControllerTest
    {
        private readonly CategoryController _categoryController;
        private readonly Mock<ICategoryService> _categoryServiceMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<IWebHostEnvironment> _webHostEnvironmentMock;
        private readonly Mock<IToastNotification> _toastNotificationMock;
        private readonly Mock<ILanguageService> _languageServiceMock;
        private readonly Mock<IApiBaseService> _apiServiceMock;
        private readonly Mock<IConfiguration> _configurationServiceMock;


        public CategoryControllerTest()
        {
            _configurationServiceMock = new();
            _apiServiceMock = new();
            _categoryServiceMock = new Mock<ICategoryService>();
            _mapperMock = new Mock<IMapper>();
            _webHostEnvironmentMock = new Mock<IWebHostEnvironment>();
            _toastNotificationMock = new Mock<IToastNotification>();
            _languageServiceMock = new();
            _categoryController = new CategoryController(_categoryServiceMock.Object, _webHostEnvironmentMock.Object,
             _mapperMock.Object, _toastNotificationMock.Object, _languageServiceMock.Object, _apiServiceMock.Object, _configurationServiceMock.Object);
        }


        //[Fact]
        //public async Task GetAllMethod_ShouldReturn_JsonResult()
        //{
        //    // Arrange
        //    int page = 1;
        //    int pageSize = 3;
        //    var testCategories = Items.GetCategories().AsQueryable();
        //    var mappedCategories = Items.ToCategoryDto(testCategories);

        //    _categoryServiceMock.Setup(x => x.GetAll())
        //        .Returns(testCategories);

        //    _categoryServiceMock.Setup(x => x.Filter(page, pageSize, null))
        //        .Returns(mappedCategories.AsQueryable().ToPagedListAsync(page, pageSize, typeof(IMongoQueryable)));

        //    _configurationServiceMock.Setup(c => c["ServiceUrls:CatalogService"]).Returns("http://localhost:5227");
        //    var categories = testCategories.Select(x => new
        //    {
        //        Id = x.Id.ToString(),
        //        x.IsActive,
        //        x.CategoryLanguages,
        //        x.CategoryMedias,
        //        x.ParentId
        //    });
        //    var serializeCategories = JsonSerializer.Serialize(categories);
        //    _apiServiceMock.Setup(a => a.SendAsync(It.IsAny<ApiRequestModel>()))
        //        .ReturnsAsync(new ApiResponseModel { Data = serializeCategories, TotalCounts = testCategories.Count() });



        //    var cats = await _categoryServiceMock.Object.Filter(page, pageSize);

        //    // Act
        //    var result = await _categoryController.GetAll(page, pageSize);


        //    // Assert
        //    var viewResult = Assert.IsType<JsonResult>(result);
        //}

        [Fact]
        public async Task CreateMethod_ModelIsNotValid()
        {
            // Arrange
            var category1 = new CategoryModel()
            {
                IsActive = true,
                ParentId = "5f8f4b8b0b4b3d1d7c9d6bce",
                CategoryLanguages = new()
                {
                    new()
                    {
                        LanguageId = "1",
                        Name = "Category for unit testing",
                        Description = "Test Description"
                    }
                }
            };
            _categoryController.ModelState.AddModelError("File", "File is required");


            // Act
            var result1 = await _categoryController.Create(category1);


            // Assert
            var viewResult1 = Assert.IsType<ViewResult>(result1);
            var model1 = Assert.IsType<CategoryModel>(viewResult1.Model);

            Assert.Equal(category1.ParentId, model1.ParentId);
            Assert.Equal(category1.CategoryLanguages.FirstOrDefault(cl => !string.IsNullOrEmpty(cl.Name)).Name, model1.CategoryLanguages.FirstOrDefault(cl => !string.IsNullOrEmpty(cl.Name)).Name);
        }

        [Fact]
        public async Task CreateMethod_ModelIsValid_ReturnsRedirectToAction()
        {
            // Arrange
            //Setup mock file using a memory stream
            var content = "Hello World from a Fake File";
            var fileName = "test.png";
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);
            writer.Write(content);
            writer.Flush();
            stream.Position = 0;


            string webRootPath = "C:\\Users\\Halil\\source\\repos\\ECommerceAppSolution\\src\\ECommerceApp.WebUI\\wwwroot";
            _webHostEnvironmentMock.Setup(x => x.WebRootPath).Returns(webRootPath);

            var category = new CategoryModel()
            {
                IsActive = true,
                ParentId = "5f8f4b8b0b4b3d1d7c9d6bce",
                CategoryLanguages = new()
                {
                    new()
                    {
                        LanguageId = "1",
                        Name = "Category for unit testing",
                        Description = "Test Description"
                    }
                },
                CategoryMedias = new()
                {
                    new()
                    {
                        LanguageCode = "en",
                        File = new FormFile(stream, 0, stream.Length, "id_from_form", fileName),
                        LanguageId = "1",
                        MediaType = 1,
                        SizeType = 1,
                        Title = "test"
                    }
                }
            };

            // Act
            var result = await _categoryController.Create(category);

            // Assert
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectToActionResult.ActionName);
            Assert.Null(redirectToActionResult.ControllerName);
        }


        private string GetJsonResponse()
        {
            return "\r\n  \"data\": [          \r\n    {\r\n      \"id\": \"651554fdee3d6b376951832d\",\r\n      \"categoryLanguages\": [\r\n        {\r\n          \"id\": null,\r\n          \"languageId\": \"650c65cad4190293145b7b25\",\r\n          \"languageCode\": \"en\",\r\n          \"name\": \"Television\",\r\n          \"description\": null,\r\n          \"sortNr\": 3\r\n        },\r\n        {\r\n          \"id\": null,\r\n          \"languageId\": \"650c6e4d35293db07f2201b1\",\r\n          \"languageCode\": \"tr\",\r\n          \"name\": \"Televizyon\",\r\n          \"description\": null,\r\n          \"sortNr\": 0\r\n        }\r\n      ],\r\n      \"isActive\": true,\r\n      \"categoryMedias\": [\r\n        {\r\n          \"id\": null,\r\n          \"languageId\": \"650c65cad4190293145b7b25\",\r\n          \"languageCode\": \"en\",\r\n          \"mediaType\": 1,\r\n          \"sizeType\": 1,\r\n          \"title\": null,\r\n          \"path\": \"4mjeiezc.ugt.jpg\"\r\n        }\r\n      ]\r\n    },\r\n    {\r\n      \"id\": \"65186ec3e031e29dbd72cee5\",\r\n      \"categoryLanguages\": [\r\n        {\r\n          \"id\": null,\r\n          \"languageId\": \"650c65cad4190293145b7b25\",\r\n          \"languageCode\": \"en\",\r\n          \"name\": \"sub category\",\r\n          \"description\": null,\r\n          \"sortNr\": 0\r\n        }\r\n      ],\r\n      \"isActive\": true,\r\n      \"categoryMedias\": [\r\n        {\r\n          \"id\": null,\r\n          \"languageId\": \"650c65cad4190293145b7b25\",\r\n          \"languageCode\": \"en\",\r\n          \"mediaType\": 1,\r\n          \"sizeType\": 1,\r\n          \"title\": null,\r\n          \"path\": \"fk4z0ryx.e4q.jpg\"\r\n        }\r\n      ]\r\n    }\r\n  ],\r\n  \"status\": true,\r\n  \"errors\": [],\r\n  \"totalCounts\": 5\r\n";
        }
    }

}