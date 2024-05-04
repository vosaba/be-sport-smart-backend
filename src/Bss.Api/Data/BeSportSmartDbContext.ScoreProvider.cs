using Bss.Api.Data.Models;
using Bss.Api.Data.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Bss.Api.Data
{
    public partial class BeSportSmartDbContext
    {
        public DbSet<ScoreProvider> ScoreProviders { get; set; }
        public DbSet<ScoreProviderInput> ScoreProviderInputs { get; set; }

        public Task<ScoreProvider?> GetScoreProvider(string name) 
        {
            return ScoreProviders.SingleOrDefaultAsync(x => x.Name == name);
        }

        public void AddScoreProvider(ScoreProvider scoreProvider, IEnumerable<Input> inputs)
        {
            Add(scoreProvider);

            foreach (var input in inputs)
            {
                Add(new ScoreProviderInput
                {
                    ScoreProvider = scoreProvider,
                    Input = input
                });
            }
        }
    }
}
