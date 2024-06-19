using Bss.Api.Data.Models;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace Bss.Api.Dtos.Input
{
    public class InputDto
    {
        [Required]
        public string Name { get; set; } = string.Empty;

        [Required]
        public InputType Type { get; set; }

        [Required]
        public InputSource InputSource { get; set; }

        public string[] Options { get; set; } = Array.Empty<string>();
    }
}
