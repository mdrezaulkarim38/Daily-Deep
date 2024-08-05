using System.Data.SQLite;
using Daily_Deep.Models;
using Daily_Deep.Interfaces;

namespace Daily_Deep.Services;
public class AccountService : IAccountService
{
    private readonly string _connectionString;
    public AccountService(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection")!;
    }

    public async Task CreateCategory(Category category)
    {
        using (SQLiteConnection connection = new SQLiteConnection(_connectionString))
        {
            string query = @"INSERT INTO Category (CategoryName, CategoryCode, UserId, CreatedAt, UpdatedAt) VALUES (@CategoryName, @CategoryCode, @UserId, @CreatedAt, @UpdatedAt)";
            using (SQLiteCommand command = new SQLiteCommand(query, connection))
            {
                command.Parameters.AddWithValue("@CategoryName", category.CategoryName);
                command.Parameters.AddWithValue("@CategoryCode", category.CategoryCode);
                command.Parameters.AddWithValue("@UserId", category.UserId);
                var nowUtc = DateTime.UtcNow;
                command.Parameters.AddWithValue("@CreatedAt", nowUtc);
                command.Parameters.AddWithValue("@UpdatedAt", nowUtc);

                connection.Open();
                await command.ExecuteNonQueryAsync();
            }
        }
    }

    public async Task<List<Category>> GetCategories(int userId)
    {
        var categories = new List<Category>();
        using (SQLiteConnection connection = new SQLiteConnection(_connectionString))
        {
            string query = "SELECT * FROM Category WHERE UserId = @UserId";
            using (SQLiteCommand command = new SQLiteCommand(query, connection))
            {
                command.Parameters.AddWithValue("@UserId", userId);
                connection.Open();
                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        var category = new Category();
                        category.Id = reader.GetInt32(0);
                        category.CategoryName = reader.GetString(1);
                        category.CategoryCode = reader.GetString(2);
                        category.UserId = reader.GetInt32(3);

                        categories.Add(category);
                    }
                }
            }
        }
        return categories;
    }


    public async Task CreateTransaction(TransactionData transactionData)
    {
        using (SQLiteConnection connection = new SQLiteConnection(_connectionString))
        {
            string query = @"INSERT INTO Transactions (TransactionDate, CategoryCode, TransactionType, Description, Amount, UserId, CreatedAt, UpdatedAt) VALUES (@TransactionDate, @CategoryCode, @TransactionType, @Description, @Amount, @UserId, @CreatedAt, @UpdatedAt)";
            using (SQLiteCommand command = new SQLiteCommand(query, connection))
            {
                command.Parameters.AddWithValue("@TransactionDate", transactionData.TransactionDate);
                command.Parameters.AddWithValue("@CategoryCode", transactionData.CategoryCode);
                command.Parameters.AddWithValue("@TransactionType", transactionData.TransactionType);
                command.Parameters.AddWithValue("@Description", transactionData.Description);
                command.Parameters.AddWithValue("@Amount", transactionData.Amount);
                command.Parameters.AddWithValue("@UserId", transactionData.UserId);
                var nowUtc = DateTime.UtcNow;
                command.Parameters.AddWithValue("@CreatedAt", nowUtc);
                command.Parameters.AddWithValue("@UpdatedAt", nowUtc);

                connection.Open();
                await command.ExecuteNonQueryAsync();
            }
        }
    }

    public async Task<List<TransactionData>> GetTransactions(int userId)
    {
        var transactionData = new List<TransactionData>();
        using (SQLiteConnection connection = new SQLiteConnection(_connectionString))
        {
            string query = "SELECT TransactionDate,(SELECT Category.CategoryName FROM Category WHERE Category.CategoryCode = Transactions.CategoryCode) as CategoryCode, TransactionType, Description, Amount  FROM Transactions WHERE UserId = @UserId";
            using (SQLiteCommand command = new SQLiteCommand(query, connection))
            {
                command.Parameters.AddWithValue("@UserId", userId);
                connection.Open();
                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        var transaction = new TransactionData();
                        transaction.TransactionDate = reader.GetDateTime(0);
                        transaction.CategoryCode = reader.GetString(1);
                        transaction.TransactionType = reader.GetString(2);
                        transaction.Description = reader.GetString(3);
                        transaction.Amount = reader.GetDecimal(4);
                        transactionData.Add(transaction);
                    }
                }
            }
        }
        return transactionData;
    }

    public async Task<Category> GetCategoryById(int id, int userId)
    {
        using (SQLiteConnection connection = new SQLiteConnection(_connectionString))
        {
            string query = "SELECT * FROM Category WHERE Id = @Id AND UserId = @userId";
            using (SQLiteCommand command = new SQLiteCommand(query, connection))
            {
                command.Parameters.AddWithValue("@Id", id);
                command.Parameters.AddWithValue("@userId", userId);
                connection.Open();
                using (var reader = await command.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        return new Category
                        {
                            Id = reader.GetInt32(0),
                            CategoryName = reader.GetString(1),
                            CategoryCode = reader.GetString(2),
                            UserId = reader.GetInt32(3)
                        };
                    }
                }
            }
        }
        return null!;
    }

    public async Task UpdateCategory(Category category)
    {
        using (SQLiteConnection connection = new SQLiteConnection(_connectionString))
        {
            string query = @"UPDATE Category SET CategoryName = @CategoryName, UpdatedAt = @UpdatedAt WHERE Id = @Id AND UserId = @userId";
            using (SQLiteCommand command = new SQLiteCommand(query, connection))
            {
                command.Parameters.AddWithValue("@CategoryName", category.CategoryName);
                command.Parameters.AddWithValue("@UpdatedAt", DateTime.UtcNow);
                command.Parameters.AddWithValue("@Id", category.Id);
                command.Parameters.AddWithValue("@userId", category.UserId);
                connection.Open();
                await command.ExecuteNonQueryAsync();
            }
        }
    }

    public async Task<bool> CanDeleteCategory(int categoryId, int userId)
    {
        using (SQLiteConnection connection = new SQLiteConnection(_connectionString))
        {
            string query = "SELECT COUNT(*) FROM Transactions WHERE Transactions.CategoryCode = (SELECT CategoryCode FROM Category WHERE Id = @Id AND Category.UserId = @userId) AND Transactions.UserId = @userId";
            using (SQLiteCommand command = new SQLiteCommand(query, connection))
            {
                command.Parameters.AddWithValue("@Id", categoryId);
                command.Parameters.AddWithValue("@userId", userId);
                connection.Open();
                var result = await command.ExecuteScalarAsync();
                return Convert.ToInt32(result) == 0;
            }
        }
    }

    public async Task DeleteCategory(int id, int userId)
    {
        using (SQLiteConnection connection = new SQLiteConnection(_connectionString))
        {
            string query = "DELETE FROM Category WHERE Id = @Id and UserId = @userId ";
            using (SQLiteCommand command = new SQLiteCommand(query, connection))
            {
                command.Parameters.AddWithValue("@Id", id);
                command.Parameters.AddWithValue("@userId", userId);
                connection.Open();
                await command.ExecuteNonQueryAsync();
            }
        }
    }

}