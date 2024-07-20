using System.ComponentModel.DataAnnotations;

namespace Bss.Component.Core.Commands.Admin.GetComputation;

public class GetComputationRequest
{
    [Required]
    public Guid Id { get; init; }
}
