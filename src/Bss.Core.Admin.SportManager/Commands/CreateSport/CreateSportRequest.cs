using Bss.Core.Bl.Enums;
using System.ComponentModel.DataAnnotations;

namespace Bss.Core.Admin.SportManager.Commands.CreateSport;

public class CreateSportRequest
{
    [Required]
    public string Sport { get; set; } = string.Empty;

    [Required]
    public ComputationEngine ComputationEngine { get; set; } = ComputationEngine.Js;

    [Required]
    public Dictionary<string, object> Variables { get; set; } = [];

    public bool Disabled { get; set; } = false;
}
