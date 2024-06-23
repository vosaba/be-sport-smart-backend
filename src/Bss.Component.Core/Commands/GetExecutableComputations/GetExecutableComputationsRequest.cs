﻿using Bss.Component.Core.Enums;
using Bss.Infrastructure.Shared.Attributes;
using System.ComponentModel.DataAnnotations;

namespace Bss.Component.Core.Commands.GetExecutableComputations;

public class GetExecutableComputationsRequest
{
    [Required]
    [EnumSubset(ComputationType.Sport, ComputationType.Metric)]
    public ComputationType Type { get; set; }

    [Required]
    public Dictionary<string, object> MeasureValues { get; set; } = [];
}
