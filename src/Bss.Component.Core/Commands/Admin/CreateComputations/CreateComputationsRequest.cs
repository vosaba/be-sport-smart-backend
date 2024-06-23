using Bss.Component.Core.Enums;
using System.ComponentModel.DataAnnotations;

namespace Bss.Component.Core.Commands.Admin.CreateComputations;

public record class CreateComputation
{
    [Required]
    public ComputationType Type { get; init; }

    [Required]
    public string Name { get; init; } = string.Empty;

    [Required]
    public string Formula { get; init; } = string.Empty;   
}

public class CreateComputationsRequest
{
    [Required]
    [MinLength(1)]
    public CreateComputation[] Computations { get; init; } = [];

    [Required]
    public ComputationEngine Engine { get; init; }

    [Required]
    public bool Disabled { get; init; }
}
