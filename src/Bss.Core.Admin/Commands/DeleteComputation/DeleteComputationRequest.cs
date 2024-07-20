using System.ComponentModel.DataAnnotations;

namespace Bss.Component.Core.Commands.Admin.DeleteComputation;

public class DeleteComputationRequest
{
    [Required]
    public Guid Id { get; init; }
}
