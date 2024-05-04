using Bss.Api.Data.Models;

namespace Bss.Api.Data.Repositories
{
    public interface IPortfolioRepository : IRepository
    {
        void AddPortfolio(Portfolio portfolio);
        Task<Portfolio> DeletePortfolio(AppUser appUser, string symbol);
    }
}