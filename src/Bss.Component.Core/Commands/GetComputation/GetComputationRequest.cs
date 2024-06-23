using System.ComponentModel.DataAnnotations;

namespace Bss.Component.Core.Commands.GetComputation;

public class GetComputationRequest
{
    [Required]
    public Guid Id { get; init; }
}
