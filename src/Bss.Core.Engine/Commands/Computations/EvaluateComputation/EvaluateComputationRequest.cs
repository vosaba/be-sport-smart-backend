using System.ComponentModel.DataAnnotations;

namespace Bss.Core.Engine.Commands.Computations.EvaluateComputation;

public class EvaluateComputationRequest
{
    [Required]
    public string Name { get; set; } = string.Empty;

    [Required]
    public Dictionary<string, string> MeasureValues { get; set; } = [];
}
