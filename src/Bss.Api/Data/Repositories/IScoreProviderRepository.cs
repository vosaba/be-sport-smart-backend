using Bss.Api.Data.Models;
using Bss.Api.Dtos.ScoreProvider;

namespace Bss.Api.Data.Repositories
{
    public interface IScoreProviderRepository : IRepository
    {
        Task<List<ScoreProvider>> GetScoreProviders(ScoreProviderFilterDto scoreProviderFilter);
        Task<ScoreProvider?> GetScoreProvider(string name);
        Task<List<string>> GetDependenstOfScoreProvider(string name);
        void AddScoreProvider(ScoreProvider scoreProvider, IEnumerable<Input> inputs);
        void UpdateScoreProvider(ScoreProvider scoreProvider, IEnumerable<Input> inputs);
        void RemoveScoreProvider(ScoreProvider scoreProvider);
    }
}
