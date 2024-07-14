using Bss.Component.Core.Enums;
using Bss.Infrastructure.Shared.Attributes;
using System.ComponentModel.DataAnnotations;

namespace Bss.Component.Core.Commands.GetAvailableComputations;

public class GetAvailableComputationsRequest
{
    [Required]
    [EnumSubset(ComputationType.Sport, ComputationType.Metric)]
    public ComputationType Type { get; set; }

    [Required]
    public Dictionary<string, string>? MeasureValues { get; set; } = [];
}
