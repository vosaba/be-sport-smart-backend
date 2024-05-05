using Bss.Api.Data.Models;
using System.ComponentModel.DataAnnotations;

namespace Bss.Api.Dtos.ScoreProvider
{
    public class ScoreProviderFilterDto
    {
        public ScoreProviderType? Type { get; set; }

        public string? Name { get; set; }

        public string? DependentOn { get; set; }

        public bool? Disabled { get; set; }
    }
}
