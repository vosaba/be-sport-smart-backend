using Bss.Component.Core.Enums;

namespace Bss.Component.Core.Dto;

public record AvailableMeasureDto 
{
    public required string Name { get; init; } = string.Empty;

    public required MeasureType Type { get; init; }

    public required MeasureSource InputSource { get; init; }

    public required string[] Options { get; init; } = [];
}