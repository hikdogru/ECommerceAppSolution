using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Diagnostics;
using Newtonsoft.Json;

namespace ECommerceApp.WebUI.Middlewares
{
    public class GlobalErrorHandlingMiddleware : IMiddleware
    {
        private readonly ILogger<GlobalErrorHandlingMiddleware> _logger;

        public GlobalErrorHandlingMiddleware(ILogger<GlobalErrorHandlingMiddleware> logger)
        {
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            if (exception.InnerException is not null)
            {
                _logger.LogError($"ExceptionType: {exception.InnerException.GetType()} ExceptionMessage: {exception.InnerException.Message}");
            }
            else
            {
                _logger.LogError($"ExceptionType: {exception.GetType()} ExceptionMessage: {exception.Message}");
            }
            
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            context.Response.ContentType = "application/json";
            var errorMessage = new
            {
                message = "An error occurred while processing your request.",
                detail = exception.Message
            };

        
            await context.Response.WriteAsync(JsonConvert.SerializeObject(errorMessage));
        }
    }
}