using Bss.Component.Core.Enums;
using Bss.Component.Core.Services.ComputationAnalyzers;
using Bss.Infrastructure.Errors.Abstractions;

namespace Bss.Component.Core.Data.Models;

public class Computation
{
    public Computation(
        string name,
        ComputationType type,
        ComputationEngine engine,
        Guid createdBy,
        bool disabled)
        => (Name, Type, Engine, CreatedBy, CreatedAt, UpdatedAt, Disabled, Availability)
        = (name, type, engine, createdBy, DateTime.UtcNow, DateTime.UtcNow, disabled, GetAvailability(type, null));

    public Computation(
        string name,
        ComputationType type,
        ComputationEngine engine,
        Guid createdBy,
        bool disabled,
        ComputationAvailability? availability)
        => (Name, Type, Engine, CreatedBy, CreatedAt, UpdatedAt, Disabled, Availability)
        = (name, type, engine, createdBy, DateTime.UtcNow, DateTime.UtcNow, disabled, GetAvailability(type, availability));

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

    public ComputationAvailability Availability { get; private set; }

    public void Update(
        ComputationType type,
        ComputationEngine engine,
        string name,
        bool disabled,
        ComputationAvailability? availability)
        => (Type, Engine, Name, Disabled, UpdatedAt, Availability) 
        = (type, engine, name, disabled, DateTime.UtcNow, GetAvailability(type, availability));

    public async Task SetFormula(string formula, IComputationAnalyzer computationAnalyzer)
    {
        Formula = formula;
        (_requiredComputations, _requiredMeasures) = await computationAnalyzer.GetComputationRequirements(this);

        await computationAnalyzer.EnsureValid(this);
    }

    public bool IsVisibleForUser(bool isSignedIn)
        => !Disabled
        && (Availability == ComputationAvailability.NoRestriction || isSignedIn && (Availability == ComputationAvailability.User));

    public bool IsExecutableByUser(bool isSignedIn)
        => !Disabled
        && (Availability == ComputationAvailability.NoRestriction || isSignedIn && (Availability == ComputationAvailability.User));

    private static ComputationAvailability GetAvailability(ComputationType type, ComputationAvailability? availability) => type switch
    {
        ComputationType.Sport => availability ?? ComputationAvailability.NoRestriction,
        ComputationType.Metric => availability ?? ComputationAvailability.User,
        ComputationType.Score => availability is not null and not ComputationAvailability.Internal
            ? throw new OperationException("Score can only be internal.", OperationErrorCodes.Forbidden) 
            : ComputationAvailability.Internal,
        _ => throw new ArgumentOutOfRangeException(nameof(type), type, null),
    };
}
