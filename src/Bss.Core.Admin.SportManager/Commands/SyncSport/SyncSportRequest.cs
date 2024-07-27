using System.ComponentModel.DataAnnotations;

namespace Bss.Core.Admin.SportManager.Commands.SyncSport;

public class SyncSportRequest
{
    [Required]
    public string Name { get; set; } = string.Empty;

    [Required]
    public Dictionary<string, object> Variables { get; set; } = [];
}