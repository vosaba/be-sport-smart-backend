using Bss.Component.Core.Enums;
using System.ComponentModel.DataAnnotations;

namespace Bss.Component.Core.Commands.Admin.CreateMeasures;

public record class CreateMeasure
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

public class CreateMeasuresRequest
{
    [Required]
    [MinLength(1)]
    public CreateMeasure[] Measures { get; init; } = [];
}
