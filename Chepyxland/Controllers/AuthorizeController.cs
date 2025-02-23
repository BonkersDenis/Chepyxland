using Chepyxland.Models;
using Chepyxland.Services;
using Microsoft.AspNetCore.Mvc;

namespace Chepyxland.Controllers;

[Route("[controller]/[action]")]
public class AuthorizeController(AuthorizeService authorizeService, JwtTokenService tokenService):Controller
{
    [HttpGet]
    public IActionResult SignIn() => View(new SignInModel());

    [HttpPost]
    public async Task<IActionResult> SignIn(SignInModel model)
    {
        if (!await authorizeService.IsUserExist(model.Login, model.Password))
            return BadRequest();

        var token = tokenService.GenerateToken(model.Login);

        var cookieOptions = new CookieOptions
                            {
                                Expires = DateTime.Now.AddDays(7), // Устанавливаем срок действия куки
                                SameSite = SameSiteMode.Strict // Ограничивает отправку куки только в рамках того же сайта
                            };

        await authorizeService.AddTokenToUser(model.Login, token);

        // Добавляем куки в ответ
        Response.Cookies.Append("Token", token, cookieOptions);

        return RedirectToAction("Index", "Files");
    }

    [HttpGet]
    public IActionResult SignUp() => View(new SignUpModel());

    [HttpPost]
    public async Task<IActionResult> SignUp(SignUpModel model)
    {
        //TODO: Check only by login
        if (await authorizeService.IsUserExist(model.Login, model.Password))
            return BadRequest();

        await authorizeService.RegisterUser(model.Login, model.UserName, model.Password);

        var token = tokenService.GenerateToken(model.Login);

        var cookieOptions = new CookieOptions
                            {
                                Expires = DateTime.Now.AddDays(7), // Устанавливаем срок действия куки
                                SameSite = SameSiteMode.Strict // Ограничивает отправку куки только в рамках того же сайта
                            };

        // Добавляем куки в ответ
        Response.Cookies.Append("Token", token, cookieOptions);

        await authorizeService.AddTokenToUser(model.Login, token);
        
        return RedirectToAction("Index", "Files");
    }
}