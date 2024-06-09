using Bss.Api.Data.Models;
using Bss.Api.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Bss.Api.Data
{
    public partial class BeSportSmartDbContext : DbContextWithEntityConfiguration, IBeSportSmartDbContext, IPortfolioRepository, IRepository, IInputRepository, IScoreProviderRepository
    {
        IQueryable<Input> IBeSportSmartDbContext.Inputs => Inputs;
        IQueryable<ScoreProvider> IBeSportSmartDbContext.ScoreProviders => ScoreProviders;
        IQueryable<Portfolio> IBeSportSmartDbContext.Portfolios => Portfolios;

        public BeSportSmartDbContext(DbContextOptions dbContextOptions)
        : base(dbContextOptions)
        {

        }

        public void Delete<T>(T entity)
            => Remove(entity);

        public void Push<T>(T entity)
            => Add(entity);

        public async Task ApplyChanges()
        {
            await SaveChangesAsync();
        }

        void IRepository.Add<T>(T aggregationRoot) => Add(aggregationRoot);

        void IRepository.Remove<T>(T aggregationRoot) => Remove(aggregationRoot);

        private EntityEntry<T> GetEntry<T>(T entity)
            where T : class =>
            base.Entry<T>(entity);
    }
}