﻿using Bss.Component.Core.Enums;

namespace Bss.Component.Core.Commands.Admin.GetMeasures;

public class GetMeasuresRequest
{
    public string? Name { get; init; }

    public MeasureType? Type { get; init; }

    public MeasureSource? InputSource { get; init; }

    public bool? Disabled { get; init; }
}