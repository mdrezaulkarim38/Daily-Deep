using Daily_Deep.Models;

namespace Daily_Deep.Interfaces;
public interface IAccountService
{
    Task CreateCategory(Category category);
    Task<Category> GetCategory();
}