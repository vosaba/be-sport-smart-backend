using Bss.Api.Data.Models;
using Bss.Api.Data.Repositories;
using Bss.Api.Dtos.ScoreProvider;
using Microsoft.EntityFrameworkCore;

namespace Bss.Api.Data
{
    public partial class BeSportSmartDbContext
    {
        public DbSet<ScoreProvider> ScoreProviders { get; set; }
        public DbSet<ScoreProviderInput> ScoreProviderInputs { get; set; }

        public Task<List<ScoreProvider>> GetScoreProviders(ScoreProviderFilterDto scoreProviderFilter)
        {
            var query = ScoreProviders.AsQueryable();

            if (scoreProviderFilter.Type.HasValue)
            {
                query = query.Where(x => x.Type == scoreProviderFilter.Type.Value);
            }

            if (!string.IsNullOrWhiteSpace(scoreProviderFilter.Name))
            {
                query = query.Where(x => x.Name.Contains(scoreProviderFilter.Name));
            }

            if (!string.IsNullOrWhiteSpace(scoreProviderFilter.DependentOn))
            {
                query = query.Where(x => x.DependentProviders.Contains(scoreProviderFilter.DependentOn));
            }

            if (scoreProviderFilter.Disabled.HasValue)
            {
                query = query.Where(x => x.Disabled == scoreProviderFilter.Disabled.Value);
            }

            return query.ToListAsync();
        }

        public Task<ScoreProvider?> GetScoreProvider(string name) 
        {
            return ScoreProviders.SingleOrDefaultAsync(x => x.Name == name);
        }

        public async Task<List<string>> GetDependenstOfScoreProvider(string name)
        {
            return await ScoreProviders
                .Where(x => x.DependentProviders.Contains(name))
                .Select(x => x.Name)
                .ToListAsync();
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

        public void UpdateScoreProvider(ScoreProvider scoreProvider, IEnumerable<Input> inputs)
        {
            Update(scoreProvider);

            foreach (var scoreProviderInput in scoreProvider.ScoreProviderInputs)
            {
                Remove(scoreProviderInput);
            }

            foreach (var input in inputs)
            {
                Add(new ScoreProviderInput
                {
                    ScoreProvider = scoreProvider,
                    Input = input
                });
            }
        }

        public void RemoveScoreProvider(ScoreProvider scoreProvider)
        {
            ScoreProviders.Remove(scoreProvider);
        }
    }
}
