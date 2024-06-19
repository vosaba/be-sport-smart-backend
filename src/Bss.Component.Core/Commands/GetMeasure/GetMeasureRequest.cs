using System.ComponentModel.DataAnnotations;

namespace Bss.Component.Core.Commands.GetMeasure;

public class GetMeasureRequest
{
    [Required]
    public string MeasureId { get; set; } = string.Empty;
}
