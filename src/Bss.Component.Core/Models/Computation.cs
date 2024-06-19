using System.Text.RegularExpressions;
using Bss.Component.Core.Enums;

namespace Bss.Component.Core.Models;

public partial class Computation
{
    public Computation(
        ComputationType type,
        string name,
        string formula,
        string createdBy)
    {
        (_requiredComputations, _requiredMeasures) = FormulaDependencies(formula);
        (Type, Name, Formula, CreatedBy, CreatedAt, UpdatedAt) = (type, name, formula, createdBy, DateTime.UtcNow, DateTime.UtcNow);
    }

    public Guid Id { get; init; }

    public ComputationType Type { get; private set; }

    public string Name { get; private set; } = string.Empty;

    public string Formula { get; private set; } = string.Empty;

    private List<string> _requiredComputations = [];
    public IReadOnlyCollection<string> RequiredComputations => _requiredComputations;

    private List<string> _requiredMeasures = [];
    public IReadOnlyCollection<string> RequiredMeasures => _requiredMeasures;

    public bool Disabled { get; private set; }

    public string CreatedBy { get; init; } = string.Empty;

    public DateTime CreatedAt { get; init; }

    public DateTime UpdatedAt { get; private set; }

    public void Update(
        ComputationType type,
        string name,
        string formula,
        bool disabled)
    {
        (_requiredComputations, _requiredMeasures) = FormulaDependencies(formula);
        (Type, Name, Formula, Disabled, UpdatedAt) = (type, name, formula, disabled, DateTime.UtcNow);
    }

    private static (List<string> computations, List<string> measures) FormulaDependencies(string formula) 
        => (ComputationsUsed()
            .Matches(formula)
            .Select(m => m.Groups[1].Value)
            .Distinct()
            .ToList(),
            MeasuresUsed()
            .Matches(formula)
            .Select(m => m.Groups[1].Value)
            .Distinct()
            .ToList());

    [GeneratedRegex(@"context\.(metrics|scores)\.(\w+)")]
    private static partial Regex ComputationsUsed();

    [GeneratedRegex(@"context\.measures\.(\w+)")]
    private static partial Regex MeasuresUsed();
}
