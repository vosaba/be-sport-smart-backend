using System.ComponentModel.DataAnnotations;

namespace Bss.Component.Core.Commands.EvaluateComputation;

public class EvaluateComputationRequest
{
    [Required]
    public string Name { get; set; } = string.Empty;

    [Required]
    public Dictionary<string, object> MeasureValues { get; set; } = [];
}
