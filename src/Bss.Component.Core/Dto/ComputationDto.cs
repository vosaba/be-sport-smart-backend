﻿using Bss.Component.Core.Enums;

namespace Bss.Component.Core.Dto;

public record ComputationDto
{
    public required Guid Id { get; init; }

    public required ComputationType Type { get; init; }

    public required string Name { get; init; } = string.Empty;

    public required string Formula { get; init; } = string.Empty;

    public required string[] RequiredComputations { get; init; } = [];

    public required string[] RequiredMeasures { get; init; } = [];

    public bool Disabled { get; init; }
}