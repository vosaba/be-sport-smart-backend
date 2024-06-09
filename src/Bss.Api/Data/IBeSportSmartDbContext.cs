using Bss.Api.Data.Models;
using Bss.Api.Data.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Bss.Api.Data
{
    public interface IBeSportSmartDbContext : IRepository
    {
        IQueryable<Input> Inputs { get; }

        IQueryable<ScoreProvider> ScoreProviders { get; }

        IQueryable<Portfolio> Portfolios { get; }
    }
}