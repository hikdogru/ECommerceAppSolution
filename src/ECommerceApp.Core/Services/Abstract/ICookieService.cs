using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerceApp.Core.Services.Abstract;

public interface ICookieService
{
    string? GetCookieValue(string cookieName);
    void SetCookieValue(string cookieName, string value, int expirationMinutes = 7200);
}
