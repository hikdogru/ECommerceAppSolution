using AutoMapper;
using ECommerceApp.Application.Services.Abstract;
using ECommerceApp.Core.Domain.Entities.Language;
using ECommerceApp.Core.Domain.Entities.Product;
using ECommerceApp.Core.DTOs;
using ECommerceApp.Core.Extensions;
using ECommerceApp.WebUI.Areas.Admin.Controllers;
using ECommerceApp.WebUI.Areas.Admin.Models.Catalog;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MongoDB.Driver.Linq;
using Moq;
using NToastNotify;

namespace WebUI.Test;

public class ProductControllerTest
{
    private readonly ProductController _productController;
    private readonly Mock<IProductService> _productServiceMock;
    private readonly Mock<ILanguageService> _languageServiceMock;
    private readonly Mock<ICategoryService> _categoryServiceMock;
    private readonly Mock<IBrandService> _brandServiceMock;
    private readonly Mock<ITagService> _tagServiceMock;
    private readonly Mock<ISpecificationService> _specificationServiceMock;
    private readonly Mock<ISpecificationValueService> _specificationValueServiceMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly Mock<IToastNotification> _toastMock;
    private readonly Mock<ILogger<ProductController>> _loggerMock;

    public ProductControllerTest()
    {
        _specificationValueServiceMock = new();
        _specificationServiceMock = new();
        _tagServiceMock = new();
        _brandServiceMock = new();
        _categoryServiceMock = new();
        _languageServiceMock = new();
        _productServiceMock = new();
        _mapperMock = new Mock<IMapper>();
        _toastMock = new();
        _loggerMock = new();
        _productController = new(_loggerMock.Object, _productServiceMock.Object,
            _mapperMock.Object, _toastMock.Object, _languageServiceMock.Object, _categoryServiceMock.Object,
            _brandServiceMock.Object, _tagServiceMock.Object, _specificationServiceMock.Object, _specificationValueServiceMock.Object);
    }


    [Fact]
    public async Task GetAllMethod_ShouldReturn_JsonResult()
    {
        // Arrange
        int page = 1;
        int pageSize = 3;
        var testProducts = GetTestProducts().AsQueryable();
        var mappedProducts = GetMappedProducts(testProducts);
        var pagedProducts = await mappedProducts.AsQueryable().ToPagedListAsync(page, pageSize, typeof(IMongoQueryable));
        var listOfProducts = pagedProducts.Select(x => x);
        _productServiceMock.Setup(x => x.Filter(page, pageSize, null))
            .ReturnsAsync(pagedProducts);

        _mapperMock.Setup(x => x.Map<IEnumerable<ProductViewModel>>(listOfProducts))
            .Returns(GetMappedProductViewModels(listOfProducts));

        // Act
        var result = await _productController.GetAll(page, pageSize);

        // Assert
        var viewResult = Assert.IsType<JsonResult>(result);
        var model = Assert.IsAssignableFrom<IEnumerable<ProductViewModel>>(viewResult.Value.GetType().GetProperty("data").GetValue(viewResult.Value, null));
        Assert.Equal(pagedProducts.Count(), model.Count());
    }









    private IEnumerable<Product> GetTestProducts()
    {
        return new List<Product>
            {

             new Product
    {
        Code = "P001",
        GroupCode = "G001",
        Price = 10.99m,
        TotalQuantity = 100,
        IsItOffSale = false,
        BrandId = "60c8e3d9c3b9d9a2c8d3d1d1",
        CategoryIds = new List<string> { "60c8e3d9c3b9d9a2c8d3d1d2", "60c8e3d9c3b9d9a2c8d3d1d3" },
        TagIds = new List<string> { "60c8e3d9c3b9d9a2c8d3d1d4", "60c8e3d9c3b9d9a2c8d3d1d5" },
        Images = new List<ProductImage>
        {
            new ProductImage { Path = "https://example.com/image1.jpg" , IsDefault = true},
            new ProductImage { Path = "https://example.com/image2.jpg" }
        },
        ProductLanguages = new List<ProductLanguage>
        {
            new ProductLanguage { LanguageId = "en-US", Name = "Product 1", Description = "This is product 1" },
            new ProductLanguage { LanguageId = "tr-TR", Name = "Ürün 1", Description = "Bu ürün 1'dir" }
        },
        ProductVariants = new List<ProductVariant>
        {
            new ProductVariant { Barcode = "P001-V1",  Quantity = 50 },
            new ProductVariant { Barcode = "P001-V2", Quantity = 50 }
        },
        ProductSpecifications = new List<ProductSpecification>
        {
            new ProductSpecification { SpecificationId = "4",  SpecificationValueId = "1" },
            new ProductSpecification { SpecificationId = "2",  SpecificationValueId = "4" }
        }
    },
        new Product
        {
            Code = "P002",
            GroupCode = "G001",
            Price = 20.99m,
            TotalQuantity = 200,
            IsItOffSale = false,
            BrandId = "60c8e3d9c3b9d9a2c8d3d1d1",
            CategoryIds = new List<string> { "60c8e3d9c3b9d9a2c8d3d1d2", "60c8e3d9c3b9d9a2c8d3d1d3" },
            TagIds = new List<string> { "60c8e3d9c3b9d9a2c8d3d1d4", "60c8e3d9c3b9d9a2c8d3d1d5" },
            Images = new List<ProductImage>
        {
            new ProductImage { Path = "https://example.com/image3.jpg", IsDefault = true },
            new ProductImage { Path = "https://example.com/image4.jpg" }
        },
            ProductLanguages = new List<ProductLanguage>
        {
            new ProductLanguage { LanguageId = "en-US", Name = "Product 2", Description = "This is product 2" },
            new ProductLanguage { LanguageId = "tr-TR", Name = "Ürün 2", Description = "Bu ürün 2'dir" }
        },
            ProductVariants = new List<ProductVariant>
        {
            new ProductVariant { Barcode = "P002-V1",  Quantity = 100 },
            new ProductVariant { Barcode = "P002-V2",  Quantity = 100 }
        },
            ProductSpecifications = new List<ProductSpecification>
        {
            new ProductSpecification { SpecificationId = "2",  SpecificationValueId = "3" },
            new ProductSpecification { SpecificationId = "3",  SpecificationValueId = "2" }
        }
        },
         new Product
        {
            Code = "P003",
            GroupCode = "G002",
            Price = 30.99m,
            TotalQuantity = 300,
            IsItOffSale = false,
            BrandId = "60c8e3d9c3b9d9a2c8d3d1d1",
            CategoryIds = new List<string> { "60c8e3d9c3b9d9a2c8d3d1d2", "60c8e3d9c3b9d9a2c8d3d1d3" },
            TagIds = new List<string> { "60c8e3d9c3b9d9a2c8d3d1d4", "60c8e3d9c3b9d9a2c8d3d1d5" },
            Images = new List<ProductImage>
        {
            new ProductImage { Path = "https://example.com/image5.jpg" , IsDefault = true},
            new ProductImage { Path = "https://example.com/image6.jpg" }
        },
            ProductLanguages = new List<ProductLanguage>
        {
            new ProductLanguage { LanguageId = "en-US", Name = "Product 3", Description = "This is product 3" },
            new ProductLanguage { LanguageId = "tr-TR", Name = "Ürün 3", Description = "Bu ürün 3'tür" }
        },
            ProductVariants = new List<ProductVariant>
        {
            new ProductVariant { Barcode = "P003-V1",  Quantity = 150 },
            new ProductVariant { Barcode = "P003-V2",  Quantity = 150 }
        },
            ProductSpecifications = new List<ProductSpecification>
        {
            new ProductSpecification { SpecificationId = "2",  SpecificationValueId = "1" },
            new ProductSpecification { SpecificationId = "1",  SpecificationValueId = "2" }
        }
        },
new Product
{
    Code = "P004",
    GroupCode = "G002",
    Price = 40.99m,
    TotalQuantity = 400,
    IsItOffSale = false,
    BrandId = "60c8e3d9c3b9d9a2c8d3d1d1",
    CategoryIds = new List<string> { "60c8e3d9c3b9d9a2c8d3d1d2", "60c8e3d9c3b9d9a2c8d3d1d3" },
    TagIds = new List<string> { "60c8e3d9c3b9d9a2c8d3d1d4", "60c8e3d9c3b9d9a2c8d3d1d5" },
    Images = new List<ProductImage>
        {
            new ProductImage { Path = "https://example.com/image7.jpg", IsDefault = true },
            new ProductImage { Path = "https://example.com/image8.jpg" }
        },
    ProductLanguages = new List<ProductLanguage>
        {
            new ProductLanguage { LanguageId = "en-US", Name = "Product 4", Description = "This is product 4" },
            new ProductLanguage { LanguageId = "tr-TR", Name = "Ürün 4", Description = "Bu ürün 4'tür" }
        },
    ProductVariants = new List<ProductVariant>
        {
            new ProductVariant { Barcode = "P004-V1",  Quantity = 200 },
            new ProductVariant { Barcode = "P004-V2",  Quantity = 200 }
        },
    ProductSpecifications = new List<ProductSpecification>
        {
            new ProductSpecification { SpecificationId = "4",  SpecificationValueId = "3" },
            new ProductSpecification { SpecificationId = "5",  SpecificationValueId = "2" }
        }
},
    new Product
    {
        Code = "P005",
        GroupCode = "G003",
        Price = 50.99m,
        TotalQuantity = 500,
        IsItOffSale = false,
        BrandId = "60c8e3d9c3b9d9a2c8d3d1d1",
        CategoryIds = new List<string> { "60c8e3d9c3b9d9a2c8d3d1d2", "60c8e3d9c3b9d9a2c8d3d1d3" },
        TagIds = new List<string> { "60c8e3d9c3b9d9a2c8d3d1d4", "60c8e3d9c3b9d9a2c8d3d1d5" },
        Images = new List<ProductImage>
        {
            new ProductImage { Path = "https://example.com/image9.jpg" },
            new ProductImage { Path = "https://example.com/image10.jpg" , IsDefault = true}
        },
        ProductLanguages = new List<ProductLanguage>
        {
            new ProductLanguage { LanguageId = "en-US", Name = "Product 5", Description = "This is product 5" },
            new ProductLanguage { LanguageId = "tr-TR", Name = "Ürün 5", Description = "Bu ürün 5'tir" }
        },
        ProductVariants = new List<ProductVariant>
        {
            new ProductVariant { Barcode = "P005-V1", Quantity = 250 },
            new ProductVariant { Barcode = "P005-V2",  Quantity = 250 }
        },
        ProductSpecifications = new List<ProductSpecification>
        {
            new ProductSpecification { SpecificationId = "2",  SpecificationValueId = "1" },
            new ProductSpecification { SpecificationId = "3",  SpecificationValueId = "3" }
        }
    },

    new Product
    {
        Code = "P006",
        GroupCode = "G003",
        Price = 60.99m,
        TotalQuantity = 600,
        IsItOffSale = false,
        BrandId = "60c8e3d9c3b9d9a2c8d3d1d1",
        CategoryIds = new List<string> { "60c8e3d9c3b9d9a2c8d3d1d2", "60c8e3d9c3b9d9a2c8d3d1d3" },
        TagIds = new List<string> { "60c8e3d9c3b9d9a2c8d3d1d4", "60c8e3d9c3b9d9a2c8d3d1d5" },
        Images = new List<ProductImage>
        {
            new ProductImage { Path = "https://example.com/image11.jpg" },
            new ProductImage { Path = "https://example.com/image12.jpg" , IsDefault = true}
        },
        ProductLanguages = new List<ProductLanguage>
        {
            new ProductLanguage { LanguageId = "en-US", Name = "Product 6", Description = "This is product 6" },
            new ProductLanguage { LanguageId = "tr-TR", Name = "Ürün 6", Description = "Bu ürün 6'dır" }
        },
        ProductVariants = new List<ProductVariant>
        {
            new ProductVariant { Barcode = "P006-V1",  Quantity = 300 },
            new ProductVariant { Barcode = "P006-V2",  Quantity = 300 }
        },
        ProductSpecifications = new List<ProductSpecification>
        {
            new ProductSpecification { SpecificationId = "1",  SpecificationValueId = "2" },
            new ProductSpecification { SpecificationId = "2",  SpecificationValueId = "3" }
        }
    }
            };
    }

    private IEnumerable<ProductDTO> GetMappedProducts(IEnumerable<Product> products)
    {
        return products.Select(x => new ProductDTO
        (
            x.Id.ToString(),
            x.Code,
            x.GroupCode,
            x.Price,
            x.TotalQuantity,
            x.IsItOffSale,
            x.BrandId,
            x.CategoryIds,
            x.TagIds,
            x.Images,
            x.ProductLanguages,
            x.TaxRate,
            x.ProductVariants,
            x.ProductSpecifications


        )).ToList();
    }

    private IEnumerable<ProductViewModel> GetMappedProductViewModels(IEnumerable<ProductDTO> products)
    {
        return products.Select(x => new ProductViewModel
        {
            Id = x.Id.ToString(),
            Code = x.Code,
            Image = x.Images is List<ProductImage> ? x.Images.FirstOrDefault(y => y.IsDefault).Path : "",
            IsItOffSale = x.IsItOffSale,
            Price = x.Price,
            TotalQuantity = x.TotalQuantity
        });
    }
}
