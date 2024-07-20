using Bss.Core.Bl.Enums;
using Bss.Infrastructure.Shared.Attributes;
using System.ComponentModel.DataAnnotations;

namespace Bss.Core.Engine.Commands.Computations.GetAvailableComputations;

public class GetAvailableComputationsRequest
{
    [Required]
    [EnumSubset(ComputationType.Sport, ComputationType.Metric)]
    public ComputationType Type { get; set; }

    [Required]
    public Dictionary<string, string>? MeasureValues { get; set; } = [];
}
