using Daily_Deep.Models;

namespace Daily_Deep.Interfaces;
public interface IHomeService
{
    Task<int> GetTransactionByType(string Type, int userId);
}
