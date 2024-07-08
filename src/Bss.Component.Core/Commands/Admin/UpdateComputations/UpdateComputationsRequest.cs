using Bss.Component.Core.Enums;
using System.ComponentModel.DataAnnotations;

namespace Bss.Component.Core.Commands.Admin.UpdateComputations;

public record class UpdateComputation
{
    [Required]
    public Guid Id { get; init; }

    [Required]
    public ComputationType Type { get; init; }

    [Required]
    public ComputationEngine Engine { get; init; }

    [Required]
    public bool Disabled { get; init; }

    [Required]
    public string Name { get; init; } = string.Empty;

    [Required]
    public string Formula { get; init; } = string.Empty;
}

public class UpdateComputationsRequest
{
    [Required]
    [MinLength(1)]
    public UpdateComputation[] Computations { get; init; } = [];
}
