using Bss.Api.Data.Models;
using Bss.Api.Dtos.ScoreProvider;

namespace Bss.Api.Mappers
{
    public static class ScoreProviderMapper
    {
        public static ScoreProviderDto ToScoreProviderDto(this ScoreProvider scoreProvider)
        {
            return new ScoreProviderDto
            {
                Type = scoreProvider.Type,
                Name = scoreProvider.Name,
                Formula = scoreProvider.Formula,
            };
        }
    }
}