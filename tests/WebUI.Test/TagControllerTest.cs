using System.Text;
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

namespace WebUI.Test;

public class TagControllerTest
{
    private readonly TagController _tagController;
    private readonly Mock<ITagService> _tagServiceMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly Mock<IToastNotification> _toastMock;
    private readonly Mock<ILogger<TagController>> _loggerMock;

    public TagControllerTest()
    {
        _tagServiceMock = new();
        _mapperMock = new Mock<IMapper>();
        _toastMock = new();
        _loggerMock = new();
        _tagController = new(_loggerMock.Object, _tagServiceMock.Object, _mapperMock.Object, _toastMock.Object);
    }

    [Fact]
    public async Task GetAllMethod_ShouldReturn_JsonResult()
    {
        // Arrange
        int page = 1;
        int pageSize = 3;
        var testTags = GetTestTags().AsQueryable();
        var mappedTags = GetMappedTags(testTags);
        var pagedTags = await mappedTags.AsQueryable().ToPagedListAsync(page, pageSize, typeof(IMongoQueryable));
        var listOfBrands = pagedTags.Select(x => x);
        _tagServiceMock.Setup(x => x.Filter(page, pageSize, null))
            .ReturnsAsync(pagedTags);

        _mapperMock.Setup(x => x.Map<IEnumerable<TagViewModel>>(listOfBrands))
            .Returns(GetMappedTagViewModels(listOfBrands));

        // Act
        var result = await _tagController.GetAll(page, pageSize);

        // Assert
        var viewResult = Assert.IsType<JsonResult>(result);
        var model = Assert.IsAssignableFrom<IEnumerable<TagViewModel>>(viewResult.Value.GetType().GetProperty("data").GetValue(viewResult.Value, null));
        Assert.Equal(pagedTags.Count(), model.Count());
    }


    [Fact]
    public async Task CreateMethod_WithValidModel_ShouldReturn_SuccessfulJsonResult()
    {
        // Arrange
        var model = new TagModel
        {
            Name = "Test Tag",
            Description = "Test Tag Description",
            IsActive = true
        };
        var tag = new Tag
        {
            Name = model.Name,
            Description = model.Description,
            IsActive = model.IsActive
        };

        _mapperMock.Setup(x => x.Map<Tag>(model)).Returns(tag);
        _tagServiceMock.Setup(x => x.Insert(tag, default)).Returns(Task.CompletedTask);
        _toastMock.Setup(x => x.AddSuccessToastMessage("Tag created successfully", null));

        // Act
        var result = await _tagController.Create(model);

        // Assert
        var jsonResult = Assert.IsType<JsonResult>(result);
        Assert.True((bool)jsonResult.Value.GetType().GetProperty("success").GetValue(jsonResult.Value, null));
        _tagServiceMock.Verify(x => x.Insert(tag, default), Times.Once);
        _toastMock.Verify(x => x.AddSuccessToastMessage("Tag created successfully", null), Times.Once);
    }


    [Fact]
    public async Task CreateMethod_WithInValidModel_ShouldReturn_UnSuccessfulJsonResult()
    {
        // Arrange
        var model = new TagModel
        {
            Description = "Test Tag Description",
            IsActive = true
        };

        _tagController.ModelState.AddModelError("Name", "The name field is required!");

        // Act
        var result = await _tagController.Create(model);

        // Assert
        var jsonResult = Assert.IsType<JsonResult>(result);
        Assert.False((bool)jsonResult.Value.GetType().GetProperty("success").GetValue(jsonResult.Value, null));

    }


    [Fact]
    public async Task Edit_Found_ReturnsSuccessResult()
    {
        // Arrange
        var tag = GetTestTags().FirstOrDefault(x => x.Id == ObjectId.Parse("5f8f4b8b0b4b3d1d7c9d6bca"));
        var id = tag?.Id;
        var tagModel = new TagModel()
        {
            Id = tag.Id.ToString(),
            Name = tag.Name,
            Description = tag.Description,
            IsActive = tag.IsActive,
        };

        _tagServiceMock.Setup(tagService => tagService.GetById((ObjectId)id)).ReturnsAsync(tag);
        _mapperMock.Setup(mapper => mapper.Map<TagModel>(tag)).Returns(tagModel);

        // Act
        var result = await _tagController.Edit((ObjectId)id);

        // Assert
        var jsonResult = Assert.IsType<JsonResult>(result);
        var data = (TagModel)jsonResult.Value.GetType().GetProperty("data").GetValue(jsonResult.Value, null);

        Assert.True((bool)jsonResult.Value.GetType().GetProperty("success").GetValue(jsonResult.Value, null));
        Assert.Same(tagModel, data);
    }


    [Fact]
    public async Task Edit_NotFound_ReturnsUnSuccessResult()
    {
        // Arrange           
        var id = ObjectId.Empty;

        _tagServiceMock.Setup(tagService => tagService.GetById(id)).ReturnsAsync((Tag?)null);
        _toastMock.Setup(toast => toast.AddErrorToastMessage("Tag is not found", null));

        // Act
        var result = await _tagController.Edit((ObjectId)id);

        // Assert
        var jsonResult = Assert.IsType<JsonResult>(result);
        Assert.False((bool)jsonResult.Value.GetType().GetProperty("success").GetValue(jsonResult.Value, null));
    }


    [Fact]
    public async Task Edit_ValidModelAndFound_ReturnsSuccessResult()
    {
        // Arrange           
        var model = new TagModel
        {
            Id = "5f8f4b8b0b4b3d1d7c9d6bca",
            Name = "Tag 111",
            Description = "updated",
            IsActive = true
        };

        var tagInDB = GetTestTags().FirstOrDefault(x => x.Id == ObjectId.Parse(model.Id));
        _tagServiceMock.Setup(tagService => tagService.GetById(tagInDB.Id)).ReturnsAsync(tagInDB);
        _mapperMock.Setup(mapper => mapper.Map(model, tagInDB)).Returns(tagInDB);
        _tagServiceMock.Setup(tagService => tagService.Update(tagInDB, false)).Returns(Task.CompletedTask);
        _toastMock.Setup(toast => toast.AddSuccessToastMessage("Tag updated successfully", null));

        // Act
        var result = await _tagController.Edit(model);

        // Assert
        var jsonResult = Assert.IsType<JsonResult>(result);
        var success = (bool)jsonResult.Value.GetType().GetProperty("success").GetValue(jsonResult.Value, null);

        Assert.True(success);
    }


    [Fact]
    public async Task Edit_InValidModel_ReturnsUnSuccessfulResult()
    {
        // Arrange
        var model = new TagModel
        {
            Description = "Test Description",
            IsActive = true
        };

        _tagController.ModelState.AddModelError("Name", "The name field is required!");

        // Act
        var result = await _tagController.Edit(model);

        // Assert
        var jsonResult = Assert.IsType<JsonResult>(result);
        Assert.False((bool)jsonResult.Value.GetType().GetProperty("success").GetValue(jsonResult.Value, null));
    }


    [Fact]
    public async Task Edit_NotFound_ReturnsUnSuccessfulResult()
    {
        // Arrange
        var model = new TagModel
        {
            Id = "5f8f4b8b0b4b3d1d7c9d6bca",
            Name = "test",
            Description = "Test Description",
            IsActive = true
        };

        _tagServiceMock.Setup(tagService => tagService.GetById(ObjectId.Parse(model.Id))).ReturnsAsync((Tag?)null);

        // Act
        var result = await _tagController.Edit(model);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }


    [Fact]
    public async Task Delete_Found_ReturnSuccessfulResult()
    {
        // Arrange           
        var id = ObjectId.Parse("5f8f4b8b0b4b3d1d7c9d6bca");

        var tagInDB = GetTestTags().FirstOrDefault(x => x.Id == id);
        _tagServiceMock.Setup(tagService => tagService.GetById(tagInDB.Id)).ReturnsAsync(tagInDB);
        _tagServiceMock.Setup(tagService => tagService.Delete(tagInDB.Id)).Returns(Task.CompletedTask);
        _toastMock.Setup(toast => toast.AddSuccessToastMessage("Tag deleted successfully", null));

        // Act
        var result = await _tagController.Delete(id);

        // Assert
        var jsonResult = Assert.IsType<JsonResult>(result);
        var success = (bool)jsonResult.Value.GetType().GetProperty("success").GetValue(jsonResult.Value, null);

        Assert.True(success);
    }


    private IEnumerable<Tag> GetTestTags()
    {
        return new List<Tag>
            {
                new() { Id = ObjectId.Parse("5f8f4b8b0b4b3d1d7c9d6bca"), Name = "Tag 1", Description = "Tag 1 desc", IsActive = false },
                new() { Id = ObjectId.Parse("5f8f4b8b0b4b3d1d7c9d6bcb"), Name = "Tag 2",Description = "Tag 2 desc",  IsActive = true },
                new() { Id = ObjectId.Parse("5f8f4b8b0b4b3d1d7c9d6bcc"), Name = "Tag 3", IsActive = true },
                new() { Id = ObjectId.Parse("5f8f4b8b0b4b3d1d7c9d6bcd"), Name = "Tag 4", Description = "Tag 4 desc" ,IsActive = false },
                new() { Id = ObjectId.Parse("5f8f4b8b0b4b3d1d7c9d6bce"), Name = "Tag 5",IsActive = true }
            };
    }

    private IEnumerable<TagDTO> GetMappedTags(IEnumerable<Tag> tags)
    {
        return tags.Select(x => new TagDTO
        (
            x.Id,
             x.Name,
             x.Description,
            x.IsActive
        )).ToList();
    }

    private IEnumerable<TagViewModel> GetMappedTagViewModels(IEnumerable<TagDTO> tags)
    {
        return tags.Select(x => new TagViewModel
        {
            Id = x.Id.ToString(),
            Name = x.Name,
            Description = x.Description,
            IsActive = x.IsActive
        });
    }
}
