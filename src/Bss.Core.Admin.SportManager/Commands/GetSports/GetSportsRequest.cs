using Bss.Core.Bl.Enums;

namespace Bss.Core.Admin.SportManager.Commands.GetSports;

public class GetSportsRequest
{
    public ComputationType Type { get; set; } = ComputationType.Sport;

    public string? Name { get; set; }
}