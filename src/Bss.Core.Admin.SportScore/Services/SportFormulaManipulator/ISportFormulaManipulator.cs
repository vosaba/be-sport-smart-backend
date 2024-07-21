using Bss.Core.Bl.Models;

namespace Bss.Core.Admin.SportScore.Services.SportFormulaManipulator;

public interface ISportFormulaManipulator
{
    Dictionary<string, double> GetSportScoreData(Computation computation);

    Task ApplyScoreDataToFormula(Computation computation, Dictionary<string, double> scoreData);
}
