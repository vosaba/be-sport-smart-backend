using System.ComponentModel.DataAnnotations;

namespace Bss.Core.Admin.SportManager.Commands.UpdateSports;

public class UpdateSportsRequest
{
    [Required]
    [MinLength(1)]
    public Sport[] Sports { get; set; } = [];

    public class Sport
    {
        [Required]
        public string Name { get; set; } = string.Empty;

        [Required]
        public Dictionary<string, object> Variables { get; set; } = [];
    }
}