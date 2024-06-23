using System.ComponentModel.DataAnnotations;

namespace Bss.Component.Core.Commands.GetExecutableComputations;

public class GetExecutableComputationsRequest
{

    [Required]
    public Dictionary<string, object> MeasureValues { get; set; } = [];
}
