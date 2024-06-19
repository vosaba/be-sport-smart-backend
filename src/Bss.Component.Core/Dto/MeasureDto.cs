using Bss.Component.Core.Enums;

namespace Bss.Component.Core.Dto;

public record MeasureDto
{
    public required Guid Id { get; init; }

    public required string Name { get; init; } = string.Empty;

    public required MeasureType Type { get; init; }

    public required MeasureSource InputSource { get; init; }

    public required string[] Options { get; init; } = [];

    public bool Disabled { get; init; }
}