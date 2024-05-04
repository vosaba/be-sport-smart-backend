using Bss.Api.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Bss.Api.Data
{
    public partial class BeSportSmartDbContext
    {
        public DbSet<Portfolio> Portfolios { get; set; }

        public void AddPortfolio(Portfolio portfolio)
        {
            Push(portfolio);
        }

        public async Task<Portfolio> DeletePortfolio(AppUser appUser, string symbol)
        {
            var portfolioModel = await Portfolios.FirstOrDefaultAsync(x => x.AppUserId == appUser.Id);

            if (portfolioModel == null)
            {
                return null;
            }

            Portfolios.Remove(portfolioModel);
            return portfolioModel;
        }
    }
}
