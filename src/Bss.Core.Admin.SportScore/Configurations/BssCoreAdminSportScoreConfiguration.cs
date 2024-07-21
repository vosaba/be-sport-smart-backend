using Bss.Infrastructure.Configuration.Abstractions;

namespace Bss.Core.Admin.SportScore.Configurations;

[Configuration]
public class BssCoreAdminSportScoreConfiguration
{
    public Dictionary<string, string[]> Patterns { get; set; } = [];
}
