using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ECommerceApp.Core.Services.Abstract;
using Microsoft.AspNetCore.Http;

namespace ECommerceApp.Core.Services.Concrete
{
    public class HttpContextCookieService : ICookieService
    {
        private readonly IHttpContextAccessor _contextAccessor;

        public HttpContextCookieService(IHttpContextAccessor contextAccessor)
        {
            _contextAccessor = contextAccessor;
        }

        /// <summary>
        /// Gets the value of a cookie by its name from the current HTTP context.
        /// </summary>
        /// <param name="cookieName">The name of the cookie to retrieve.</param>
        /// <returns>The value of the cookie, or null if the cookie does not exist.</returns>
        public string? GetCookieValue(string cookieName)
        {
            var context = _contextAccessor.HttpContext;
            string? value = null;
            if (context is HttpContext && context.Request.Cookies.TryGetValue(cookieName, out value))
            {

            }

            return value;
        }

        /// <summary>
        /// Sets the value of a cookie with the specified name and expiration time in minutes.
        /// </summary>
        /// <param name="cookieName">The name of the cookie.</param>
        /// <param name="value">The value to be stored in the cookie.</param>
        /// <param name="expirationMinutes">The number of minutes until the cookie expires.</param>
        public void SetCookieValue(string cookieName, string value, int expirationMinutes = 7200)
        {
            var context = _contextAccessor.HttpContext;
            if (context is HttpContext)
            {

                var cookieOptions = new CookieOptions
                {
                    Expires = DateTime.Now.AddMinutes(expirationMinutes),
                };

                context.Response.Cookies.Append(cookieName, value, cookieOptions);
            }
        }
    }
}