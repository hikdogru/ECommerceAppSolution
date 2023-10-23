using Microsoft.AspNetCore.Mvc;
using Asp.Versioning;
using ECommerceApp.WebApiAbstraction.Controllers;
using ECommerceApp.Application.Services.Abstract;
using ECommerceApp.AuthService.Models.User;
using ECommerceApp.Application.Models.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace ECommerceApp.AuthService.Controllers
{
    [ApiVersion("1.0")]
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class AuthController : ApiControllerBase
    {
        private readonly IUserService _userService;

        public AuthController(IUserService userService)
        {
            _userService = userService;
        }

        [Route("Register")]
        [HttpPost]
        public async Task<IActionResult> Register([FromBody] RegistrationRequest registrationModel)
        {
            var errorMessage = await _userService.RegisterAsync(registrationModel);
            if (errorMessage is not null)
            {
                return CreateActionResult<object>(null, statusCode: StatusCodes.Status400BadRequest, new(errorMessage));
            }

            await _userService.AssignRole(registrationModel.Email, "Customer");
            return CreateActionResult<object>(null, statusCode: StatusCodes.Status200OK);
        }


        [Route("Login")]
        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginRequest loginModel)
        {
            var loginResponse = await _userService.LoginAsync(loginModel);
            if (loginResponse.TokenModel is null)
            {
                return CreateActionResult<object>(null, statusCode: StatusCodes.Status400BadRequest, new("UserName or password is incorrect!"));
            }

            return CreateActionResult<object>(loginResponse, statusCode: StatusCodes.Status200OK);
        }

        [HttpGet]
        [Authorize(Roles = "Customer", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [Route("Test")]
        public IActionResult Test()
        {
            return Ok("Success!");
        }
    }
}