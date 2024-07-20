using Bss.Core.Bl.Enums;

namespace Bss.Core.Admin.Dto;

public record MeasureDto
{
    public required Guid Id { get; init; }

    public required string Name { get; init; } = string.Empty;

    public required MeasureType Type { get; init; }

    public required double? MinValue { get; init; }

    public required double? MaxValue { get; init; }

    public required int Order { get; init; }

    public required MeasureAvailability Availability { get; init; }

    public required string[] Options { get; init; } = [];

    public bool Disabled { get; init; }
}