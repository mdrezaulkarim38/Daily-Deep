using System.Data.SQLite;
using Daily_Deep.Interfaces;

namespace Daily_Deep.Services;

public class HomeService : IHomeService
{
    private readonly string _connectionString;
    public HomeService(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection")!;
    }

    public async Task<int> GetTransactionByType(string type, int userId)
    {
        using (SQLiteConnection connection = new SQLiteConnection(_connectionString))
        {
            string query = "SELECT SUM(Amount) as Amount FROM Transactions WHERE TransactionType = @Type AND UserId = @UserId";
            await using var command = new SQLiteCommand(query,connection);
            command.Parameters.AddWithValue("@Type", type);
            command.Parameters.AddWithValue("@UserId", userId);
            await connection.OpenAsync();
            await using var reader = await command.ExecuteReaderAsync();

            if (await reader.ReadAsync())
            {
                if (!reader.IsDBNull(0))
                {
                    return Convert.ToInt32(reader.GetDouble(0));
                }
            }
        }
        return 0;
    }
}
