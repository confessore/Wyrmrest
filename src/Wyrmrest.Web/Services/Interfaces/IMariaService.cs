using System.Threading.Tasks;

namespace Wyrmrest.Web.Services.Interfaces
{
    public interface IMariaService
    {
        Task<bool> AccountExistsAsync(string username);
        Task CreateNewAccountAsync(string username, string password, byte expansion);
        Task UpdatePasswordAsync(string username, string password);
        Task<int> GetAccountIdAsync(string username);
        Task<bool> BanExistsAsync(int id);
        Task<bool> BanActiveAsync(int id);
        Task AddBanAsync(int id, int banDate, int unbanDate, string bannedBy, string banReason, bool active);
        Task RemoveBanAsync(int id);
    }
}
