using Bss.Core.Bl.Enums;
using System.ComponentModel.DataAnnotations;

namespace Bss.Component.Core.Commands.Admin.UpdateMeasure;

public class UpdateMeasureRequest
{
    [Required]
    public Guid Id { get; init; }

    [Required]
    public string Name { get; init; } = string.Empty;

    [Required]
    public MeasureType Type { get; init; }

    public int? MinValue { get; init; }

    public int? MaxValue { get; init; }

    public int Order { get; init; } = 1000;

    public MeasureAvailability Availability { get; init; } = MeasureAvailability.NoRestriction;

    [Required]
    public string[] Options { get; init; } = [];

    [Required]
    public bool Disabled { get; init; }
}
