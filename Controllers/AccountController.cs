using Daily_Deep.Models;
using Daily_Deep.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;
using Daily_Deep.ViewModel;

namespace Daily_Deep.Controllers;

[Authorize]
[Route("[controller]")]
public class AccountController : Controller
{
    private readonly ILogger<AccountController> _logger;
    private readonly IAccountService _accountService;
    public AccountController(ILogger<AccountController> logger, IAccountService accountService)
    {
        _logger = logger;
        _accountService = accountService;
    }

    [HttpGet]
    public IActionResult Index()
    {
        var fullName = User.FindFirst("FullName")?.Value;
        ViewBag.FullName = fullName;
        var userId = User.FindFirst("UserId")?.Value;
        ViewBag.UserId = userId;
        return View();
    }

    [HttpGet("Category")]
    public async Task<IActionResult> Category()
    {
        var userId = Convert.ToInt32(User.FindFirst("UserId")?.Value);
        var categories = await _accountService.GetCategories(userId);
        ViewBag.Categories = categories;
        return View(new Daily_Deep.Models.Category());
    }

    [HttpPost("Category")]
    public async Task<IActionResult> Category(Category category)
    {
        var userId = Convert.ToInt32(User.FindFirst("UserId")?.Value);
        category.UserId = userId;
        if (ModelState.IsValid)
        {
            await _accountService.CreateCategory(category);
            return RedirectToAction("Category");
        }
        return RedirectToAction("Category");
    }

    [HttpGet("Transaction")]
    public async Task<IActionResult> Transaction()
    {
        var fullName = User.FindFirst("FullName")?.Value;
        ViewBag.FullName = fullName;
        var userId = Convert.ToInt32(User.FindFirst("UserId")?.Value);
        var categories = await _accountService.GetCategories(userId);
        var selectList = new SelectList(categories, "CategoryCode", "CategoryName");

        var viewModel = new TransactionViewModel
        {
            CategorySelectList = selectList
        };

        ViewData["selectList"] = selectList;
        return View(viewModel);
    }


    /* [HttpPost("Transaction")]
    public async Task<IActionResult> Transaction(TransactionData transactionData)
    {
        var userId = Convert.ToInt32(User.FindFirst("UserId")?.Value);
        return View();
    } */

    [HttpGet("Report")]
    public IActionResult Report()
    {
        var userId = Convert.ToInt32(User.FindFirst("UserId")?.Value);
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View("Error!");
    }
}
