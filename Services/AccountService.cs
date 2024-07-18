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
}