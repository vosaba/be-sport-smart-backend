using Bss.Component.Core.Enums;
using System.ComponentModel.DataAnnotations;

namespace Bss.Component.Core.Commands.CreateComputation;

public class CreateComputationRequest
{
    [Required]
    public ComputationType Type { get; init; }

    [Required]
    public ComputationEngine Engine { get; init; }

    [Required]
    public string Name { get; init; } = string.Empty;

    [Required]
    public string Formula { get; init; } = string.Empty;
}
