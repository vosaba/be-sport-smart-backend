using Bss.Infrastructure.Configuration.Abstractions;

namespace Bss.Core.Admin.SportManager.Configurations;

[Configuration]
public class BssCoreAdminSportManagerConfiguration
{
    public string JsSportFormulaTemplate { get; set; } = string.Empty;
}
