using System.Text;
using AutoMapper;
using ECommerceApp.Application.Services.Abstract;
using ECommerceApp.Core.Domain.Entities.Product;
using ECommerceApp.Core.Domain.Entities.System;
using ECommerceApp.Core.DTOs;
using ECommerceApp.Core.Extensions;
using ECommerceApp.Core.Helpers;
using ECommerceApp.WebUI.Areas.Admin.Controllers;
using ECommerceApp.WebUI.Areas.Admin.Models.Catalog;
using ECommerceApp.WebUI.Areas.Admin.Models.System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using MongoDB.Driver.Linq;
using Moq;
using NToastNotify;

namespace WebUI.Test;

public class ParameterControllerTest
{

    private readonly ParameterController _parameterController;
    private readonly Mock<IParameterService> _parameterServiceMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly Mock<IToastNotification> _toastMock;
    private readonly Mock<ILogger<ParameterController>> _loggerMock;

    public ParameterControllerTest()
    {
        _parameterServiceMock = new();
        _mapperMock = new Mock<IMapper>();
        _toastMock = new();
        _loggerMock = new();
        _parameterController = new(_loggerMock.Object, _parameterServiceMock.Object, _mapperMock.Object, _toastMock.Object);
    }


    [Fact]
    public async Task GetAllMethod_ShouldReturn_JsonResult()
    {
        // Arrange
        int page = 1;
        int pageSize = 3;
        var testParameters = GetTestParameters().AsQueryable();
        var mappedParameters = GetMappedParameters(testParameters);
        var pagedParameters = await mappedParameters.AsQueryable().ToPagedListAsync(page, pageSize, typeof(IMongoQueryable));
        var listOfBrands = pagedParameters.Select(x => x);
        _parameterServiceMock.Setup(x => x.Filter(page, pageSize, null))
            .ReturnsAsync(pagedParameters);

        _mapperMock.Setup(x => x.Map<IEnumerable<ParameterViewModel>>(listOfBrands))
            .Returns(GetMappedParameterViewModels(listOfBrands));

        // Act
        var result = await _parameterController.GetAll(page, pageSize);

        // Assert
        var viewResult = Assert.IsType<JsonResult>(result);
        var model = Assert.IsAssignableFrom<IEnumerable<ParameterViewModel>>(viewResult.Value.GetType().GetProperty("data").GetValue(viewResult.Value, null));
        Assert.Equal(pagedParameters.Count(), model.Count());
    }


    [Fact]
    public async Task CreateMethod_WithValidModel_ShouldReturn_SuccessfulJsonResult()
    {
        // Arrange
        var model = new ParameterModel
        {
            Name = "Test Parameter",
            Value = "Test Parameter Value"
        };
        var parameter = new Parameter
        {
            Name = model.Name,
            Value = model.Value,
        };

        _mapperMock.Setup(x => x.Map<Parameter>(model)).Returns(parameter);
        _parameterServiceMock.Setup(x => x.Insert(parameter, default)).Returns(Task.CompletedTask);
        _toastMock.Setup(x => x.AddSuccessToastMessage("Parameter created successfully", null));

        // Act
        var result = await _parameterController.Create(model);

        // Assert
        var jsonResult = Assert.IsType<JsonResult>(result);
        Assert.True((bool)jsonResult.Value.GetType().GetProperty("success").GetValue(jsonResult.Value, null));
        _parameterServiceMock.Verify(x => x.Insert(parameter, default), Times.Once);
        _toastMock.Verify(x => x.AddSuccessToastMessage("Parameter created successfully", null), Times.Once);
    }


    [Fact]
    public async Task CreateMethod_WithInValidModel_ShouldReturn_UnSuccessfulJsonResult()
    {
        // Arrange
        var model = new ParameterModel
        {
            Value = "Test Parameter value"
        };

        _parameterController.ModelState.AddModelError("Name", "The name field is required!");

        // Act
        var result = await _parameterController.Create(model);

        // Assert
        var jsonResult = Assert.IsType<JsonResult>(result);
        Assert.False((bool)jsonResult.Value.GetType().GetProperty("success").GetValue(jsonResult.Value, null));

    }


    [Fact]
    public async Task Edit_Found_ReturnsSuccessResult()
    {
        // Arrange
        var parameter = GetTestParameters().FirstOrDefault(x => x.Id == ObjectId.Parse("5f8f4b8b0b4b3d1d7c9d6bca"));
        var id = parameter?.Id;
        var parameterModel = new ParameterModel()
        {
            Id = parameter.Id.ToString(),
            Name = parameter.Name,
            Value = parameter.Value
        };

        _parameterServiceMock.Setup(parameterService => parameterService.GetById((ObjectId)id)).ReturnsAsync(parameter);
        _mapperMock.Setup(mapper => mapper.Map<ParameterModel>(parameter)).Returns(parameterModel);

        // Act
        var result = await _parameterController.Edit((ObjectId)id);

        // Assert
        var jsonResult = Assert.IsType<JsonResult>(result);
        var data = (ParameterModel)jsonResult.Value.GetType().GetProperty("data").GetValue(jsonResult.Value, null);

        Assert.True((bool)jsonResult.Value.GetType().GetProperty("success").GetValue(jsonResult.Value, null));
        Assert.Same(parameterModel, data);
    }


    [Fact]
    public async Task Edit_NotFound_ReturnsUnSuccessResult()
    {
        // Arrange           
        var id = ObjectId.Empty;

        _parameterServiceMock.Setup(parameterService => parameterService.GetById(id)).ReturnsAsync((Parameter?)null);
        _toastMock.Setup(toast => toast.AddErrorToastMessage("Parameter is not found", null));

        // Act
        var result = await _parameterController.Edit((ObjectId)id);

        // Assert
        var jsonResult = Assert.IsType<JsonResult>(result);
        Assert.False((bool)jsonResult.Value.GetType().GetProperty("success").GetValue(jsonResult.Value, null));
    }


    [Fact]
    public async Task Edit_ValidModelAndFound_ReturnsSuccessResult()
    {
        // Arrange           
        var model = new ParameterModel
        {
            Id = "5f8f4b8b0b4b3d1d7c9d6bca",
            Name = "Parameter 111",
            Value = "updated"
        };

        var parameterInDB = GetTestParameters().FirstOrDefault(x => x.Id == ObjectId.Parse(model.Id));
        _parameterServiceMock.Setup(parameterService => parameterService.GetById(parameterInDB.Id)).ReturnsAsync(parameterInDB);
        _mapperMock.Setup(mapper => mapper.Map(model, parameterInDB)).Returns(parameterInDB);
        _parameterServiceMock.Setup(parameterService => parameterService.Update(parameterInDB, false)).Returns(Task.CompletedTask);
        _toastMock.Setup(toast => toast.AddSuccessToastMessage("Parameter updated successfully", null));

        // Act
        var result = await _parameterController.Edit(model);

        // Assert
        var jsonResult = Assert.IsType<JsonResult>(result);
        var success = (bool)jsonResult.Value.GetType().GetProperty("success").GetValue(jsonResult.Value, null);

        Assert.True(success);
    }


    [Fact]
    public async Task Edit_InValidModel_ReturnsUnSuccessfulResult()
    {
        // Arrange
        var model = new ParameterModel
        {
            Value = "Test Value"
        };

        _parameterController.ModelState.AddModelError("Name", "The name field is required!");

        // Act
        var result = await _parameterController.Edit(model);

        // Assert
        var jsonResult = Assert.IsType<JsonResult>(result);
        Assert.False((bool)jsonResult.Value.GetType().GetProperty("success").GetValue(jsonResult.Value, null));
    }


    [Fact]
    public async Task Edit_NotFound_ReturnsUnSuccessfulResult()
    {
        // Arrange
        var model = new ParameterModel
        {
            Id = "5f8f4b8b0b4b3d1d7c9d6bca",
            Name = "test",
            Value = "Test Value"
        };

        _parameterServiceMock.Setup(parameterService => parameterService.GetById(ObjectId.Parse(model.Id))).ReturnsAsync((Parameter?)null);

        // Act
        var result = await _parameterController.Edit(model);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }


    [Fact]
    public async Task Delete_Found_ReturnSuccessfulResult()
    {
        // Arrange           
        var id = ObjectId.Parse("5f8f4b8b0b4b3d1d7c9d6bca");

        var parameterInDB = GetTestParameters().FirstOrDefault(x => x.Id == id);
        _parameterServiceMock.Setup(parameterService => parameterService.GetById(parameterInDB.Id)).ReturnsAsync(parameterInDB);
        _parameterServiceMock.Setup(parameterService => parameterService.Delete(parameterInDB.Id)).Returns(Task.CompletedTask);
        _toastMock.Setup(toast => toast.AddSuccessToastMessage("Parameter deleted successfully", null));

        // Act
        var result = await _parameterController.Delete(id);

        // Assert
        var jsonResult = Assert.IsType<JsonResult>(result);
        var success = (bool)jsonResult.Value.GetType().GetProperty("success").GetValue(jsonResult.Value, null);

        Assert.True(success);
    }


    private IEnumerable<Parameter> GetTestParameters()
    {
        return new List<Parameter>
            {
                new() { Id = ObjectId.Parse("5f8f4b8b0b4b3d1d7c9d6bca"), Name = "Parameter 1", Value = "Parameter 1 value" },
                new() { Id = ObjectId.Parse("5f8f4b8b0b4b3d1d7c9d6bcb"), Name = "Parameter 2",Value = "Parameter 2 value" },
                new() { Id = ObjectId.Parse("5f8f4b8b0b4b3d1d7c9d6bcc"), Name = "Parameter 3", Value = "Parameter 3 value" },
                new() { Id = ObjectId.Parse("5f8f4b8b0b4b3d1d7c9d6bcd"), Name = "Parameter 4", Value = "Parameter 4 value"  },
                new() { Id = ObjectId.Parse("5f8f4b8b0b4b3d1d7c9d6bce"), Name = "Parameter 5"}
            };
    }

    private IEnumerable<ParameterDTO> GetMappedParameters(IEnumerable<Parameter> parameters)
    {
        return parameters.Select(x => new ParameterDTO
        (
            x.Id,
             x.Name,
             x.Value
        )).ToList();
    }

    private IEnumerable<ParameterViewModel> GetMappedParameterViewModels(IEnumerable<ParameterDTO> parameters)
    {
        return parameters.Select(x => new ParameterViewModel
        {
            Id = x.Id.ToString(),
            Name = x.Name,
            Value = x.Value
        });
    }

}
