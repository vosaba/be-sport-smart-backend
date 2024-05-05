using Bss.Api.Data.Models;

namespace Bss.Api.Dtos.ScoreProvider
{
    public class ScoreProviderListItemDto
    {
        public ScoreProviderType Type { get; set; }

        public string Name { get; set; } = string.Empty;

        public string[] DependentOnProviders { get; set; } = Array.Empty<string>();

        public bool Disabled { get; set; }
    }
}
