using Bss.Core.Bl.Enums;
using System.ComponentModel.DataAnnotations;

namespace Bss.Component.Core.Commands.Admin.CreateComputation;

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

    [Required]
    public bool Disabled { get; init; }

    public ComputationAvailability? Availability { get; init; }
}
