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

    [HttpGet("EditCategory/{id}")]
    public async Task<IActionResult> EditCategory(int id)
    {
        var userId = Convert.ToInt32(User.FindFirst("UserId")?.Value);
        var category = await _accountService.GetCategoryById(id, userId);
        if (category == null)
        {
            return NotFound();
        }
        return View(category);
    }

    [HttpPost("EditCategory/{id}")]
    public async Task<IActionResult> EditCategory(Category category)
    {
        var userId = Convert.ToInt32(User.FindFirst("UserId")?.Value);
        category.UserId = userId;
        if (ModelState.IsValid)
        {
            await _accountService.UpdateCategory(category);
            return RedirectToAction("Category");
        }
        return View(category);
    }

    [HttpPost("DeleteCategory/{id}")]
    public async Task<IActionResult> DeleteCategory(int id)
    {
        var userId = Convert.ToInt32(User.FindFirst("UserId")?.Value);
        if (await _accountService.CanDeleteCategory(id, userId))
        {
            await _accountService.DeleteCategory(id,userId);
            return RedirectToAction("Category");
        }
        else
        {
            // Handle the case where the category cannot be deleted
            TempData["ErrorMessage"] = "Category cannot be deleted as it is associated with transactions.";
            return RedirectToAction("Category");
        }
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


    [HttpPost("Transaction")]
    public async Task<IActionResult> Transaction(TransactionViewModel model)
    {
        if (ModelState.IsValid)
        {
            var userId = Convert.ToInt32(User.FindFirst("UserId")?.Value);

            var transactionData = new TransactionData
            {
                TransactionDate = model.TransactionDate,
                CategoryCode = model.CategoryCode,
                TransactionType = model.TransactionType,
                Description = model.Description,
                Amount = model.Amount,
                UserId = userId
            };

            await _accountService.CreateTransaction(transactionData);
            return RedirectToAction("Transaction");
        }

        var categories = await _accountService.GetCategories(Convert.ToInt32(User.FindFirst("UserId")?.Value));
        model.CategorySelectList = new SelectList(categories, "CategoryCode", "CategoryName");
        ViewData["selectList"] = model.CategorySelectList;

        return View(model);
    }


    [HttpGet("Report")]
    public async Task<IActionResult> Report()
    {
        var userId = Convert.ToInt32(User.FindFirst("UserId")?.Value);
        var transactionData = await _accountService.GetTransactions(userId);
        return View(transactionData);
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View("Error!");
    }
}
