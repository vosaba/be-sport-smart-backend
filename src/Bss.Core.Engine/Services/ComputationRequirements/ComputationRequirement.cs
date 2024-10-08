namespace Bss.Core.Engine.Services.ComputationRequirements;

public class ComputationRequirement
{
    public required string ComputationName { get; init; } = string.Empty;

    public required List<string> DirectMeasureRequirements { get; init; } = [];

    public required List<string> InheritMeasureRequirements { get; init; } = [];
}