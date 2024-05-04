using Bss.Api.Data.Models;

namespace Bss.Api.Data.Repositories
{
    public interface IScoreProviderRepository : IRepository
    {
        Task<ScoreProvider?> GetScoreProvider(string name);
        void AddScoreProvider(ScoreProvider scoreProvider, IEnumerable<Input> inputs);
    }
}
