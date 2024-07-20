using System.ComponentModel.DataAnnotations;

namespace Bss.Component.Core.Commands.Admin.DeleteMeasure;

public class DeleteMeasureRequest
{
    [Required]
    public Guid Id { get; init; }
}
