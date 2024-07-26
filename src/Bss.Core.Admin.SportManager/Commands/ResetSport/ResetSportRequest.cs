using System.ComponentModel.DataAnnotations;

namespace Bss.Core.Admin.SportManager.Commands.ResetSport;

public class CreateSportRequest
{
    [Required]
    public string Sport { get; set; } = string.Empty;

    [Required]
    public Dictionary<string, object> Variables { get; set; } = [];
}