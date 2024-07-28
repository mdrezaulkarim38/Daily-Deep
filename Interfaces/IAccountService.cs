using Daily_Deep.Models;

namespace Daily_Deep.Interfaces;
public interface IAccountService
{
    Task CreateCategory(Category category);
    Task<List<Category>> GetCategories(int userId);
    Task CreateTransaction(TransactionData transactionData);
    Task<List<TransactionData>> GetTransactions(int userId);
    Task<Category> GetCategoryById(int id, int userId);
    Task UpdateCategory(Category category);
    Task<bool> CanDeleteCategory(int categoryId, int userId);
    Task DeleteCategory(int id, int userId);
}
