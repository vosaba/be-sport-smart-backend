using Bss.Component.Core.Enums;
using System.ComponentModel.DataAnnotations;

namespace Bss.Component.Core.Commands.GetAvailableMeasures;

public class GetAvailableMeasuresRequest
{
    [Required]
    [MinLength(1)]
    public MeasureSource[] Sources { get; set; } = [];
}
