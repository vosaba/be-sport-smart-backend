using Bss.Component.Core.Enums;

namespace Bss.Component.Core.Dto;

public record AvailableMeasureDto 
{
    public required string Name { get; init; } = string.Empty;

    public required MeasureType Type { get; init; }

    public required double? MinValue { get; init; }

    public required double? MaxValue { get; init; }

    public required MeasureAvailability Availability { get; init; }

    public required bool MeasurableByUser { get; init; }

    public required string[] Options { get; init; } = [];
}