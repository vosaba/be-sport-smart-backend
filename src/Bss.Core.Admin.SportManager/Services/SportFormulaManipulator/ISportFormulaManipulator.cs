namespace Bss.Core.Admin.SportManager.Services.SportFormulaManipulator;

public interface ISportFormulaManipulator
{
    Dictionary<string, object> GetFormulaVariables(string formula);

    string ApplyVariablesToFormula(string formula, Dictionary<string, object> variables);

    string CreateFormulaUsingData(Dictionary<string, object> variables);
}
