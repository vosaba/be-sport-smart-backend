using Bss.Core.Bl.Models;
using Bss.Core.Engine.Services.MeasureValues;
using Bss.Infrastructure.Errors.Abstractions;
using Bss.Infrastructure.Shared.Abstractions;

namespace Bss.Core.Engine.Services.ComputationRequirements;

public interface IComputationRequirementService
{
    bool IsRequiredMeasureProvided(Computation computation, IEnumerable<MeasureValue> measureValues);

    void EnsureRequiredMeasureProvided(Computation computation, IEnumerable<MeasureValue> measureValues);

    IEnumerable<Computation> FilterOutComputationsWithMissingMeasures(IEnumerable<Computation> computations, IEnumerable<MeasureValue> measureValues);

    void RefreshMeasureRequirementsCache();
}

public class ComputationRequirementService(
    ILocalCacheCollection<Computation> computationCacheCollection,
    ILocalCacheCollection<ComputationRequirement> computationRequirementCacheCollection)
    : IComputationRequirementService
{
    public bool IsRequiredMeasureProvided(Computation computation, IEnumerable<MeasureValue> measureValues)
    {
        var computationRequirement = GetComputationRequirements()
            .Where(x => x.ComputationName == computation.Name)
            .Single();

        return computationRequirement.InheritMeasureRequirements.All(x => measureValues.Any(y => y.Name == x));
    }

    public void EnsureRequiredMeasureProvided(Computation computation, IEnumerable<MeasureValue> measureValues)
    {
        var computationRequirement = GetComputationRequirements()
            .Where(x => x.ComputationName == computation.Name)
            .Single();

        foreach (var measureRequirement in computationRequirement.InheritMeasureRequirements)
        {
            if (!measureValues.Any(x => x.Name == measureRequirement))
            {
                throw new OperationException($"Computation '{computation.Name}' requires measure '{measureRequirement}' to be provided.", OperationErrorCodes.InvalidRequest);
            }
        }
    }

    public IEnumerable<Computation> FilterOutComputationsWithMissingMeasures(IEnumerable<Computation> computations, IEnumerable<MeasureValue> measureValues) 
        => computations.Where(x => IsRequiredMeasureProvided(x, measureValues));

    public void RefreshMeasureRequirementsCache()
    {
        var computations = computationCacheCollection.GetAll();
        var computationRequirements = new List<ComputationRequirement>();

        foreach (var computation in computations)
        {
            var requiredMeasures = GetRequiredMeasuresForComputation(computation, computations);
            computationRequirements.Add(new ComputationRequirement
            {
                ComputationName = computation.Name,
                InheritMeasureRequirements = requiredMeasures.ToList()
            });
        }

        computationRequirementCacheCollection.Overwrite(computationRequirements);
    }

    private HashSet<string> GetRequiredMeasuresForComputation(
        Computation computation,
        IEnumerable<Computation> allComputations,
        HashSet<string>? visitedComputations = null)
    {
        visitedComputations ??= [];

        if (visitedComputations.Contains(computation.Name))
        {
            return [];
        }

        visitedComputations.Add(computation.Name);

        var requiredMeasures = new HashSet<string>(computation.RequiredMeasures);

        foreach (var requiredComputationName in computation.RequiredComputations)
        {
            var requiredComputation = allComputations.SingleOrDefault(c => c.Name == requiredComputationName);

            if (requiredComputation != null)
            {
                var nestedRequiredMeasures = GetRequiredMeasuresForComputation(requiredComputation, allComputations, visitedComputations);
                requiredMeasures.UnionWith(nestedRequiredMeasures);
            }
        }

        return requiredMeasures;
    }

    private IEnumerable<ComputationRequirement> GetComputationRequirements()
    {
        if (computationRequirementCacheCollection.IsEmpty)
        {
            RefreshMeasureRequirementsCache();
        }

        return computationRequirementCacheCollection.GetAll();
    }
}