namespace Bss.Component.Core.Services.ComputationRequirements;

public class ComputationRequirement
{
    public string ComputationName { get; init; } = string.Empty;

    public List<string> InheritMeasureRequirements { get; init; } = [];
}