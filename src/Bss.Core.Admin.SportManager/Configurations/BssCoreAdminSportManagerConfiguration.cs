using Bss.Core.Bl.Enums;
using Bss.Infrastructure.Configuration.Abstractions;

namespace Bss.Core.Admin.SportManager.Configurations;

[Configuration]
public class BssCoreAdminSportManagerConfiguration
{
    public Dictionary<ComputationEngine, string> SportFormulaTemplateNames { get; set; } = [];
}
