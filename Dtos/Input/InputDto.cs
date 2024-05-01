using api.Models;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace api.Dtos.Input
{
    public class InputDto
    {
        [Required]
        public string Name { get; set; } = string.Empty;

        [Required]
        public InputType Type { get; set; }

        [NotNull]
        public string[] Options { get; set; } = Array.Empty<string>();
    }
}
