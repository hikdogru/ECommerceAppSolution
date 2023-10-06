using ECommerceApp.Core.Services.Abstract;
using Microsoft.AspNetCore.Http;
using System;
using System.Globalization;
using System.Threading.Tasks;

namespace ECommerceApp.WebUI.Middlewares;


/// <summary>
/// Middleware for setting the user's preferred language as a cookie if it doesn't exist.
/// </summary>
public class UserLanguageMiddleware : IMiddleware
{

    private readonly ICookieService _cookieService;

    public UserLanguageMiddleware(ICookieService cookieService)
    {
        _cookieService = cookieService;
    }


    /// <summary>
    /// Invokes the middleware and sets the user's preferred language as a cookie if it doesn't exist.
    /// </summary>
    /// <param name="context">The current HTTP context.</param>
    /// <param name="next">The next middleware in the pipeline.</param>
    /// <returns>A task that represents the completion of the middleware pipeline.</returns>
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        string languageCookieName = "EcommerceApp_UserLanguage";
        // Control if cookie does not exists
        if (!context.Request.Cookies.TryGetValue(languageCookieName, out _))
        {
            // Get the user's current culture
            CultureInfo userCulture = CultureInfo.CurrentCulture;
            // Get the user's preferred language
            string userLanguage = userCulture.TwoLetterISOLanguageName;
            int sevenDaysInMinutes = 60 * 24 * 7;
            var cookieExpirationMinute = DateTime.Now.AddMinutes(sevenDaysInMinutes);
            _cookieService.SetCookieValue(languageCookieName, userLanguage, sevenDaysInMinutes);
        }

        await next(context);
    }
}
