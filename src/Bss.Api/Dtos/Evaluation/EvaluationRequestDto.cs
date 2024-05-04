using System.ComponentModel.DataAnnotations;

namespace Bss.Api.Dtos.Evaluation
{
    public class EvaluationRequestDto
    {
        [Required]
        public string Name { get; set; } = string.Empty;

        [Required]
        public Dictionary<string, string> Inputs { get; set; } = new Dictionary<string, string>();
    }
}
