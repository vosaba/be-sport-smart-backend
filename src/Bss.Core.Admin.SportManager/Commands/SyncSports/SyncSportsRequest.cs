using System.ComponentModel.DataAnnotations;

namespace Bss.Core.Admin.SportManager.Commands.SyncSports;

public class SyncSportsRequest
{
    [Required]
    public string[] Names { get; set; } = [];
}