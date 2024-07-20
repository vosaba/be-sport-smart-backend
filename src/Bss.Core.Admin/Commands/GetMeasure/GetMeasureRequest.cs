using Bss.Core.Bl.Enums;
using System.ComponentModel.DataAnnotations;

namespace Bss.Component.Core.Commands.Admin.GetMeasure;

public class GetMeasureRequest
{
    [Required]
    public Guid Id { get; init; }
}
