using Bss.Component.Core.Enums;
using System.ComponentModel.DataAnnotations;

namespace Bss.Component.Core.Commands.Admin.CreateMeasure;

public class CreateMeasureRequest
{
    [Required]
    public string Name { get; init; } = string.Empty;

    [Required]
    public MeasureType Type { get; init; }

    public double? MinValue { get; init; }

    public double? MaxValue { get; init; }

    public int Order { get; init; } = 1000;

    public MeasureAvailability Availability { get; init; } = MeasureAvailability.NoRestriction;

    [Required]
    public string[] Options { get; init; } = [];

    [Required]
    public bool Disabled { get; init; }
}
