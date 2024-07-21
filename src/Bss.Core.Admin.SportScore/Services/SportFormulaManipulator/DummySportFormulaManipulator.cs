using Bss.Core.Bl.Models;

namespace Bss.Core.Admin.SportScore.Services.SportFormulaManipulator;

public class DummySportFormulaManipulator : ISportFormulaManipulator
{
    public Dictionary<string, double> GetSportScoreData(Computation computation)
    {
        throw new NotImplementedException();
    }

    public Task ApplyScoreDataToFormula(Computation computation, Dictionary<string, double> scoreData)
    {
        throw new NotImplementedException();
    }
}