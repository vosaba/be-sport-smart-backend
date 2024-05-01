using api.Models;
using Microsoft.EntityFrameworkCore;

namespace api.Data
{
    public interface IApplicationDBContext
    {
        DbSet<Input> Inputs { get; set; }
        DbSet<ScoreProvider> ScoreProviders { get; set; }
        DbSet<ScoreProviderInput> ScoreProviderInputs { get; set; }

        DbSet<Comment> Comments { get; set; }
        DbSet<Portfolio> Portfolios { get; set; }
        DbSet<Stock> Stocks { get; set; }
    }
}