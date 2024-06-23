using Bss.Component.Core.Enums;
using Bss.Component.Core.Services.ComputationAnalyzers;

namespace Bss.Component.Core.Models;

public class Computation
{
    public Computation(
        string name,
        ComputationType type,
        ComputationEngine engine,
        Guid createdBy,
        bool disabled) 
        => (Name, Type, Engine, CreatedBy, CreatedAt, UpdatedAt, Disabled)
        = (name, type, engine, createdBy, DateTime.UtcNow, DateTime.UtcNow, disabled);

    public Guid Id { get; init; }

    public ComputationType Type { get; private set; }

    public string Name { get; private set; } = string.Empty;

    public string Formula { get; private set; } = string.Empty;

    private List<string> _requiredComputations = [];
    public IReadOnlyCollection<string> RequiredComputations => _requiredComputations;

    private List<string> _requiredMeasures = [];
    public IReadOnlyCollection<string> RequiredMeasures => _requiredMeasures;

    public ComputationEngine Engine { get; private set; }

    public bool Disabled { get; private set; }

    public Guid CreatedBy { get; init; }

    public DateTime CreatedAt { get; init; }

    public DateTime UpdatedAt { get; private set; }

    public void Update(ComputationType type, ComputationEngine engine, string name, bool disabled) 
        => (Type, Engine, Name, Disabled, UpdatedAt) = (type, engine, name, disabled, DateTime.UtcNow);

    public async Task SetFormula(string formula, IComputationAnalyzer computationAnalyzer)
    {
        Formula = formula;
        (_requiredComputations, _requiredMeasures) = await computationAnalyzer.GetComputationRequirements(this);

        await computationAnalyzer.EnsureValid(this);
    }
}
