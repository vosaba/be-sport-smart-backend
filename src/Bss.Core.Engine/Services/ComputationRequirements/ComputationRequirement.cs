namespace Bss.Core.Engine.Services.ComputationRequirements;

public class ComputationRequirement
{
    public string ComputationName { get; init; } = string.Empty;

    public List<string> InheritMeasureRequirements { get; init; } = [];
}