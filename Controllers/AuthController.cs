using System.Security.Claims;
using Daily_Deep.Models;
using Daily_Deep.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace Daily_Deep.Controllers;

[Route("[controller]")]
public class AuthController : Controller
{
    private readonly ILogger<AuthController> _logger;
    private readonly IAuthService _authService;
    public AuthController(ILogger<AuthController> logger, IAuthService authService)
    {
        _logger = logger;
        _authService = authService;
    }
    [HttpGet("Login")]
    public IActionResult Login()
    {
        if (User.FindFirst("FullName")?.Value != null)
        {
            return RedirectToAction("Index", "Home");
        }
        return View();
    }
    [HttpPost("Login")]
    public async Task<IActionResult> Login(string username, string password)
    {
        if (ModelState.IsValid)
        {
            var user = await _authService.GetUser(username);
            if (user != null && user.Password == password)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.Username!),
                    new Claim("FullName", user.FullName!),
                    new Claim("UserId", user.Id.ToString())
                };
                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));
                TempData["SuccessMessage"] = "Login successful!";
                return RedirectToAction("Index", "Home");
            }
        }
        TempData["ErrorMessage"] = "Login Unsuccessful!.Try again";
        return View();
    }
    [HttpGet("Register")]
    public IActionResult Register()
    {
        if (User.FindFirst("FullName")?.Value != null)
        {
            return RedirectToAction("Index", "Home");
        }
        return View();
    }
    [HttpPost("Register")]
    public async Task<IActionResult> Register(Users user)
    {
        if (ModelState.IsValid)
        {
            await _authService.CreateUser(user);
            TempData["SuccessMessage"] = "Registration successful! Please login.";
            return RedirectToAction("Login");
        }
        TempData["SuccessMessage"] = "Registration Unsuccessful! Please Try again.";
        return View(user);
    }

    [HttpGet("Logout")]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        TempData["SuccessMessage"] = "Logout successful!";
        return RedirectToAction("Login");
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View("Error!");
    }
}