using Bss.Core.Bl.Enums;

namespace Bss.Core.Admin.SportManager.Dto;

public class SportDto
{
    public ComputationType Type { get; set; } = ComputationType.Sport;

    public string Name { get; set; } = string.Empty;

    public Dictionary<string, object> Variables { get; set; } = [];

    public string Formula { get; set; } = string.Empty;

    public bool Disabled { get; set; }
}
