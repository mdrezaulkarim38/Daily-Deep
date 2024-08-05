using System.Data.SQLite;
using Daily_Deep.Models;
using Daily_Deep.Interfaces;

namespace Daily_Deep.Services;

public class HomeService : IHomeService
{
    private readonly string _connectionString;
    public HomeService(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection")!;
    }

    public async Task<int> GetTransactionByType(string Type, int userId)
    {
        using (SQLiteConnection connection = new SQLiteConnection(_connectionString))
        {
            string query = "SELECT sum(Amount) as Amount FROM Transactions WHERE TransactionType = @Type AND UserId = @userId";
            using (SQLiteCommand command = new SQLiteCommand(query, connection))
            {
                command.Parameters.AddWithValue("@Type", Type);
                command.Parameters.AddWithValue("@userId", userId);
                connection.Open();
                using (var reader = await command.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        int value = reader.GetInt32(0);
                        return value;
                    }
                }
            }
        }
        return 0;
    }
}