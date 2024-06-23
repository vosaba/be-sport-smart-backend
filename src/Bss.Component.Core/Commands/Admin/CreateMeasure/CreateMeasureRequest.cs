using Bss.Component.Core.Enums;
using System.ComponentModel.DataAnnotations;

namespace Bss.Component.Core.Commands.Admin.CreateMeasure;

public class CreateMeasureRequest
{
    [Required]
    public string Name { get; init; } = string.Empty;

    [Required]
    public MeasureType Type { get; init; }

    [Required]
    public MeasureSource InputSource { get; init; }

    [Required]
    public string[] Options { get; init; } = [];

    [Required]
    public bool Disabled { get; init; }
}
