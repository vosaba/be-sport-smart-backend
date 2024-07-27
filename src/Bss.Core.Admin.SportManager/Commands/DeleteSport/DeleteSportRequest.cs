using System.ComponentModel.DataAnnotations;

namespace Bss.Core.Admin.SportManager.Commands.DeleteSport;

public class DeleteSportRequest
{
    [Required]
    public string Name { get; set; } = string.Empty;
}
