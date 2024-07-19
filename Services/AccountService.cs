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
            string query = "INSERT INTO Category (CategoryName, CategoryCode, UserId) VALUES (@CategoryName, @CategoryCode, @UserId)";
            using(SQLiteCommand command = new SQLiteCommand(query, connection))
            {
                command.Parameters.AddWithValue("@CategoryName", category.CategoryName);
                command.Parameters.AddWithValue("@CategoryCode", category.CategoryCode);
                command.Parameters.AddWithValue("@UserId", category.UserId);

                connection.Open();
                await command.ExecuteNonQueryAsync();
            }
        }
    }

    public async Task<Category> GetCategory()
    {
        using (SQLiteConnection connection = new SQLiteConnection(_connectionString))
        {
            string query = "SELECT * FROM Category WHERE UserId = @UserId";
            using(SQLiteCommand command = new SQLiteCommand(query, connection))
            {
                command.Parameters.AddWithValue("@UserId", 1);
                connection.Open();
                using (var reader = await command.ExecuteReaderAsync())
                {
                    if(await reader.ReadAsync())
                    {
                        return new Category
                        {
                            Id=reader.GetInt32(0),
                            CategoryName = reader.GetString(1),
                            CategoryCode = reader.GetInt32(2),
                            UserId = reader.GetInt32(3)
                        };
                    }
                }
            }
        }
        return null!;
    }
}