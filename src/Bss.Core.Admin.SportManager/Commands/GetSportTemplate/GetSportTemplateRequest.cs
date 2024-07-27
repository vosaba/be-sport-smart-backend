using Bss.Core.Bl.Enums;

namespace Bss.Core.Admin.SportManager.Commands.GetSportTemplate;

public class GetSportTemplateRequest
{
    public ComputationEngine Engine { get; set; } = ComputationEngine.Js;
}