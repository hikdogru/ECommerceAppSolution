using AutoMapper;
using ECommerceApp.Application.Services.Abstract;
using ECommerceApp.Core.Domain;
using ECommerceApp.Core.Domain.Entities;
using ECommerceApp.Core.DTOs;
using ECommerceApp.Core.Extensions;
using ECommerceApp.WebUI.Areas.Admin.Controllers;
using ECommerceApp.WebUI.Models.Category;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MongoDB.Driver.Linq;
using Moq;
using NToastNotify;

namespace WebUI.Test
{
    public class CategoryControllerTest
    {
        private readonly CategoryController _categoryController;
        private readonly Mock<ICategoryService> _categoryServiceMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<IWebHostEnvironment> _webHostEnvironmentMock;
        private readonly Mock<IToastNotification> _toastNotificationMock;


        public CategoryControllerTest()
        {
            _categoryServiceMock = new Mock<ICategoryService>();
            _mapperMock = new Mock<IMapper>();
            _webHostEnvironmentMock = new Mock<IWebHostEnvironment>();
            _toastNotificationMock = new Mock<IToastNotification>();
            _categoryController = new CategoryController(_categoryServiceMock.Object, _webHostEnvironmentMock.Object, _mapperMock.Object, _toastNotificationMock.Object);
        }


        [Fact]
        public async Task GetAllMethod_ShouldReturn_JsonResult()
        {
            // Arrange
            int page = 1;
            int pageSize = 3;
            var testCategories = Items.GetCategories().AsQueryable();
            var mappedCategories = Items.ToCategoryDto(testCategories);

            _categoryServiceMock.Setup(x => x.GetAll())
                .Returns(testCategories);

            _categoryServiceMock.Setup(x => x.Filter(page, pageSize, null))
                .Returns(mappedCategories.AsQueryable().ToPagedListAsync(page, pageSize, typeof(IMongoQueryable)));


            var cats = await _categoryServiceMock.Object.Filter(page, pageSize);

            // Act
            var result = await _categoryController.GetAll(page, pageSize);


            // Assert
            var viewResult = Assert.IsType<JsonResult>(result);
        }

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
    }
}