using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using ECommerceApp.Application.Services.Abstract;
using ECommerceApp.Core.Domain.Entities.Product;
using ECommerceApp.Core.DTOs;
using ECommerceApp.Core.Extensions;
using ECommerceApp.Core.Helpers;
using ECommerceApp.WebUI.Areas.Admin.Controllers;
using ECommerceApp.WebUI.Areas.Admin.Models.Catalog;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using MongoDB.Driver.Linq;
using Moq;
using NToastNotify;

namespace WebUI.Test
{
    public class BrandControllerTest
    {
        private readonly BrandController _brandController;
        private readonly Mock<IBrandService> _brandServiceMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<IToastNotification> _toastMock;
        private readonly Mock<IWebHostEnvironment> _envMock;
        private readonly Mock<ILogger<BrandController>> _loggerMock;

        public BrandControllerTest()
        {
            _brandServiceMock = new Mock<IBrandService>();
            _mapperMock = new Mock<IMapper>();
            _toastMock = new Mock<IToastNotification>();
            _envMock = new();
            _loggerMock = new();
            _brandController = new BrandController(_loggerMock.Object, _brandServiceMock.Object, _mapperMock.Object, _toastMock.Object, _envMock.Object);
        }

        [Fact]
        public async Task GetAllMethod_ShouldReturn_JsonResult()
        {
            // Arrange
            int page = 1;
            int pageSize = 3;
            var testBrands = GetTestBrands().AsQueryable();
            var mappedBrands = GetMappedBrands(testBrands);
            var pagedBrands = await mappedBrands.AsQueryable().ToPagedListAsync(page, pageSize, typeof(IMongoQueryable));
            var listOfBrands = pagedBrands.Select(x => x);
            _brandServiceMock.Setup(x => x.Filter(page, pageSize, null))
                .ReturnsAsync(pagedBrands);

            _mapperMock.Setup(x => x.Map<IEnumerable<BrandViewModel>>(listOfBrands))
                .Returns(GetMappedBrandViewModels(listOfBrands));

            // Act
            var result = await _brandController.GetAll(page, pageSize);

            // Assert
            var viewResult = Assert.IsType<JsonResult>(result);
            var model = Assert.IsAssignableFrom<IEnumerable<BrandViewModel>>(viewResult.Value.GetType().GetProperty("data").GetValue(viewResult.Value, null));
            Assert.Equal(pagedBrands.Count(), model.Count());
        }


        [Fact]
        public async Task CreateMethod_WithValidModel_ShouldReturn_SuccessfulJsonResult()
        {
            // Arrange
            var model = new BrandModel
            {
                Name = "Test Brand",
                Description = "Test Description",
                IsActive = true,
                File = new FormFile(new MemoryStream(Encoding.UTF8.GetBytes("This is a dummy file")), 0, 0, "Data", "dummy.txt")
            };
            var parentPath = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.Parent.FullName;
            var wwwrootPath = Directory.GetParent(parentPath).FullName + "/src/ECommerceApp.WebUI/wwwroot";
            var logoUrl = await FileHelper.SaveFile(model.File, wwwrootPath);
            var brand = new Brand
            {
                Name = model.Name,
                Description = model.Description,
                IsActive = model.IsActive,
                LogoUrl = logoUrl
            };
            _envMock.Setup(x => x.WebRootPath).Returns(wwwrootPath);
            _mapperMock.Setup(x => x.Map<Brand>(model)).Returns(brand);
            _brandServiceMock.Setup(x => x.Insert(brand, default)).Returns(Task.CompletedTask);
            _toastMock.Setup(x => x.AddSuccessToastMessage("Brand created successfully", null));

            // Act
            var result = await _brandController.Create(model);

            // Assert
            var jsonResult = Assert.IsType<JsonResult>(result);
            Assert.True((bool)jsonResult.Value.GetType().GetProperty("success").GetValue(jsonResult.Value, null));
            _brandServiceMock.Verify(x => x.Insert(brand, default), Times.Once);
            _toastMock.Verify(x => x.AddSuccessToastMessage("Brand created successfully", null), Times.Once);
            if (model.File is not null)
            {
                _envMock.Verify(x => x.WebRootPath, Times.Once);
                _mapperMock.Verify(x => x.Map<Brand>(model), Times.Once);
                _brandServiceMock.Verify(x => x.Insert(brand, default), Times.Once);
            }
        }



        [Fact]
        public async Task CreateMethod_WithInValidModel_ShouldReturn_UnSuccessfulJsonResult()
        {
            // Arrange
            var model = new BrandModel
            {
                Description = "Test Description",
                IsActive = true
            };

            _brandController.ModelState.AddModelError("Name", "The name field is required!");

            // Act
            var result = await _brandController.Create(model);

            // Assert
            var jsonResult = Assert.IsType<JsonResult>(result);
            Assert.False((bool)jsonResult.Value.GetType().GetProperty("success").GetValue(jsonResult.Value, null));

        }


        [Fact]
        public async Task Edit_BrandFound_ReturnsSuccessResult()
        {
            // Arrange
            var brand = GetTestBrands().FirstOrDefault(x => x.Id == ObjectId.Parse("5f8f4b8b0b4b3d1d7c9d6bca"));
            var id = brand?.Id;
            var brandModel = new BrandModel()
            {
                Name = brand.Name,
                Description = brand.Description,
                IsActive = brand.IsActive,
                LogoUrl = brand.LogoUrl,
                Id = brand.Id.ToString()
            };

            _brandServiceMock.Setup(brandService => brandService.GetById((ObjectId)id)).ReturnsAsync(brand);
            _mapperMock.Setup(mapper => mapper.Map<BrandModel>(brand)).Returns(brandModel);

            // Act
            var result = await _brandController.Edit((ObjectId)id);

            // Assert
            var jsonResult = Assert.IsType<JsonResult>(result);
            var data = (BrandModel)jsonResult.Value.GetType().GetProperty("data").GetValue(jsonResult.Value, null);

            Assert.True((bool)jsonResult.Value.GetType().GetProperty("success").GetValue(jsonResult.Value, null));
            Assert.Same(brandModel, data);
        }


        [Fact]
        public async Task Edit_BrandNotFound_ReturnsUnSuccessResult()
        {
            // Arrange           
            var id = ObjectId.Empty;

            _brandServiceMock.Setup(brandService => brandService.GetById(id)).ReturnsAsync((Brand?)null);
            _toastMock.Setup(toast => toast.AddErrorToastMessage("Brand is not found", null));

            // Act
            var result = await _brandController.Edit((ObjectId)id);

            // Assert
            var jsonResult = Assert.IsType<JsonResult>(result);
            Assert.False((bool)jsonResult.Value.GetType().GetProperty("success").GetValue(jsonResult.Value, null));
        }



        [Fact]
        public async Task Edit_ValidModelAndBrandFound_ReturnsSuccessResult()
        {
            // Arrange           
            var model = new BrandModel
            {
                Id = "5f8f4b8b0b4b3d1d7c9d6bca",
                Name = "Brand 111",
                Description = "updated",
                IsActive = true
            };

            var brandInDB = GetTestBrands().FirstOrDefault(x => x.Id == ObjectId.Parse(model.Id));
            _brandServiceMock.Setup(brandService => brandService.GetById(brandInDB.Id)).ReturnsAsync(brandInDB);
            _mapperMock.Setup(mapper => mapper.Map(model, brandInDB)).Returns(brandInDB);
            _brandServiceMock.Setup(brandService => brandService.Update(brandInDB, false)).Returns(Task.CompletedTask);
            _toastMock.Setup(toast => toast.AddSuccessToastMessage("Brand updated successfully", null));

            // Act
            var result = await _brandController.Edit(model);

            // Assert
            var jsonResult = Assert.IsType<JsonResult>(result);
            var success = (bool)jsonResult.Value.GetType().GetProperty("success").GetValue(jsonResult.Value, null);

            Assert.True(success);
        }

        [Fact]
        public async Task Edit_InValidModel_ReturnsUnSuccessfulResult()
        {
            // Arrange
            var model = new BrandModel
            {
                Description = "Test Description",
                IsActive = true
            };

            _brandController.ModelState.AddModelError("Name", "The name field is required!");

            // Act
            var result = await _brandController.Edit(model);

            // Assert
            var jsonResult = Assert.IsType<JsonResult>(result);
            Assert.False((bool)jsonResult.Value.GetType().GetProperty("success").GetValue(jsonResult.Value, null));
        }


        [Fact]
        public async Task Edit_BrandNotFound_ReturnsUnSuccessfulResult()
        {
            // Arrange
            var model = new BrandModel
            {
                Id = "5f8f4b8b0b4b3d1d7c9d6bca",
                Name = "test",
                Description = "Test Description",
                IsActive = true
            };

            _brandServiceMock.Setup(brandService => brandService.GetById(ObjectId.Parse(model.Id))).ReturnsAsync((Brand?)null);

            // Act
            var result = await _brandController.Edit(model);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }


        [Fact]
        public async Task Delete_BrandFound_ReturnSuccessfulResult()
        {
            // Arrange           
            var id = ObjectId.Parse("5f8f4b8b0b4b3d1d7c9d6bca");

            var brandInDB = GetTestBrands().FirstOrDefault(x => x.Id == id);
            _brandServiceMock.Setup(brandService => brandService.GetById(brandInDB.Id)).ReturnsAsync(brandInDB);
            _brandServiceMock.Setup(brandService => brandService.Delete(brandInDB.Id)).Returns(Task.CompletedTask);
            _toastMock.Setup(toast => toast.AddSuccessToastMessage("Brand deleted successfully", null));

            // Act
            var result = await _brandController.Delete(id);

            // Assert
            var jsonResult = Assert.IsType<JsonResult>(result);
            var success = (bool)jsonResult.Value.GetType().GetProperty("success").GetValue(jsonResult.Value, null);

            Assert.True(success);
        }



        [Fact]
        public async Task Delete_BrandNotFound_ReturnsUnSuccessfulResult()
        {
            // Arrange
            var id = ObjectId.Parse("5f8f4b8b0b4b3d1d7c9d6bca");
            _brandServiceMock.Setup(brandService => brandService.GetById(id)).ReturnsAsync((Brand?)null);

            // Act
            var result = await _brandController.Delete(id);

            // Assert
            var jsonResult = Assert.IsType<JsonResult>(result);
            var success = (bool)jsonResult.Value.GetType().GetProperty("success").GetValue(jsonResult.Value, null);
            Assert.False(success);
        }
        private IEnumerable<Brand> GetTestBrands()
        {
            return new List<Brand>
            {
                new() { Id = ObjectId.Parse("5f8f4b8b0b4b3d1d7c9d6bca"), Name = "Brand 1", Description = "Brand 1 desc", IsActive = false, LogoUrl = "sydwewe.png" },
                new() { Id = ObjectId.Parse("5f8f4b8b0b4b3d1d7c9d6bcb"), Name = "Brand 2",Description = "Brand 2 desc",  IsActive = true, LogoUrl = "aaedwewe.png"},
                new() { Id = ObjectId.Parse("5f8f4b8b0b4b3d1d7c9d6bcc"), Name = "Brand 3", IsActive = true, LogoUrl = "vfgwewe.png" },
                new() { Id = ObjectId.Parse("5f8f4b8b0b4b3d1d7c9d6bcd"), Name = "Brand 4", Description = "Brand 4 desc" ,IsActive = false, LogoUrl = "xtyydwewe.png"},
                new() { Id = ObjectId.Parse("5f8f4b8b0b4b3d1d7c9d6bce"), Name = "Brand 5",IsActive = true, LogoUrl = "tyuydwewe.png" }
            };
        }

        private IEnumerable<BrandDTO> GetMappedBrands(IEnumerable<Brand> brands)
        {
            return brands.Select(x => new BrandDTO
            (
                x.Id,
                 x.Name,
                 x.Description,
                x.LogoUrl,
                x.IsActive
            )).ToList();
        }

        private IEnumerable<BrandViewModel> GetMappedBrandViewModels(IEnumerable<BrandDTO> brands)
        {
            return brands.Select(x => new BrandViewModel
            {
                Id = x.Id.ToString(),
                Name = x.Name,
                Description = x.Description,
                IsActive = x.IsActive,
                LogoUrl = x.LogoUrl
            });
        }
    }
}