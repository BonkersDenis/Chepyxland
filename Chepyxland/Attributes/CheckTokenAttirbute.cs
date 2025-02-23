using Chepyxland.Data;
using Chepyxland.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Chepyxland.Attributes;

public class CheckTokenAttribute:Attribute, IAuthorizationFilter
{
    private readonly JwtTokenService _jwtTokenService = new();

    public void OnAuthorization(AuthorizationFilterContext context)
    {
        // Получаем токен из куки
        var token = context.HttpContext.Request.Cookies["Token"];

        if (string.IsNullOrEmpty(token))
        {
            context.Result = new RedirectToActionResult("SignIn", "Authorize", null);
            return;
        }

        var claimsPrincipal = _jwtTokenService.ValidateToken(token);

        if (claimsPrincipal == null)
        {
            context.Result = new RedirectToActionResult("SignIn", "Authorize", null);
        }
        
        //TODO: Check token in database
    }
}