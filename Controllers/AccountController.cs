using System.Security.Claims;
using Daily_Deep.Models;
using Daily_Deep.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;

namespace Daily_Deep.Controllers;

[Authorize]
[Route("[controller]")]
public class AccountController : Controller
{
    private readonly ILogger<AccountController> _logger;
    private readonly IAccountService _accountService;
    public AccountController (ILogger<AccountController> logger, IAccountService accountService)
    {
        _logger = logger;
        _accountService = accountService;
    }
    [HttpGet]
    public IActionResult Index()
    {
        var fullName = User.FindFirst("FullName")?.Value;
        ViewBag.FullName = fullName;
        return View();
    }

    [HttpGet("Category")]
    public IActionResult Category()
    {

        return View();
    }
    [HttpPost("Category")]
    public async Task<IActionResult> Category(Category category)
    {
        var userId = 1; //User.FindFirst("Id")!.Value;
        category.UserId = Convert.ToInt32(userId);
        if(ModelState.IsValid)
        {
            await _accountService.CreateCategory(category);
            return View();
        }
        return View(category);
    }

    [HttpGet("Transaction")]
    public IActionResult Transaction()
    {
        return View();
    }
    [HttpGet("Report")]
    public IActionResult Report()
    {
        return View();
    }
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View("Error!");
    }
}