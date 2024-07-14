using Bss.Component.Core.Enums;

namespace Bss.Component.Core.Dto;

public record AvailableComputationDto
{
    public required ComputationType Type { get; init; }

    public required string Name { get; init; } = string.Empty;

    public required ComputationAvailability Availability { get; init; }
}
