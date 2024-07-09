using System.Threading.Tasks;
using Daily_Deep.Models;


namespace Daily_Deep.Interfaces
{
    public interface IAccountService
    {
        Task CreateUser(Users user);
        Task<Users> GetUser(string username);
    }
}