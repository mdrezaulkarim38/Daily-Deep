using System.Data.SQLite;
using Daily_Deep.Models;
using Daily_Deep.Interfaces;
namespace Daily_Deep.Services;

public class AuthService : IAuthService
{
    private readonly string _connectionString;

    public AuthService(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection")!;
    }

    public async Task CreateUser(Users user)
    {
        using (SQLiteConnection connection = new SQLiteConnection(_connectionString))
        {
            string query = "INSERT INTO Users (FullName, Username, Email, Password) VALUES (@FullName, @Username, @Email, @Password)";
            using (SQLiteCommand command = new SQLiteCommand(query, connection))
            {
                command.Parameters.AddWithValue("@FullName", user.FullName);
                command.Parameters.AddWithValue("@Username", user.Username);
                command.Parameters.AddWithValue("@Email", user.Email);
                command.Parameters.AddWithValue("@Password", user.Password);

                connection.Open();
                await command.ExecuteNonQueryAsync();
            }
        }
    }

    public async Task<Users> GetUser(string username)
    {
        using (SQLiteConnection connection = new SQLiteConnection(_connectionString))
        {
            string query = "SELECT * FROM Users WHERE Username = @Username";
            using (SQLiteCommand command = new SQLiteCommand(query, connection))
            {
                command.Parameters.AddWithValue("@Username", username);

                connection.Open();
                using (var reader = await command.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        return new Users
                        {
                            Id = reader.GetInt32(0),
                            FullName = reader.GetString(1),
                            Username = reader.GetString(2),
                            Email = reader.IsDBNull(3) ? null : reader.GetString(3),
                            Password = reader.GetString(4)
                        };
                    }
                }
            }
        }

        return null!;
    }
}