using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Daily_Deep.Models;
using Microsoft.AspNetCore.Authorization;
using Daily_Deep.Interfaces;

namespace Daily_Deep.Controllers;
[Authorize]
public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IHomeService _homeService;

    public HomeController(ILogger<HomeController> logger, IHomeService homeService)
    {
        _logger = logger;
        _homeService = homeService;
    }

    public IActionResult Index()
    {
        var fullName = User.FindFirst("FullName")?.Value;
        ViewBag.FullName = fullName;
        var userId = Convert.ToInt32(User.FindFirst("UserId")?.Value);
        var Income = _homeService.GetTransactionByType("income", userId);
        ViewBag.Income = Income;
        var Expense = _homeService.GetTransactionByType("expense", userId);
        ViewBag.Expense = Expense;
        return View();
    }

    [AllowAnonymous]
    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
