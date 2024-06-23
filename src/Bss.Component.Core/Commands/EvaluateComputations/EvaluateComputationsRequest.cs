using Bss.Component.Core.Enums;
using Bss.Infrastructure.Shared.Attributes;
using System.ComponentModel.DataAnnotations;

namespace Bss.Component.Core.Commands.EvaluateComputations;

public class EvaluateComputationsRequest
{
    [Required]
    [EnumSubset(ComputationType.Sport, ComputationType.Metric)]
    public ComputationType Type { get; set; }

    [Required]
    public Dictionary<string, string> MeasureValues { get; set; } = [];
}
