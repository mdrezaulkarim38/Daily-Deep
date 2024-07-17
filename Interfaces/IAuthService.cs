using Daily_Deep.Models;

namespace Daily_Deep.Interfaces;
public interface IAuthService
{
    Task CreateUser(Users user);
    Task<Users> GetUser(string username);
}