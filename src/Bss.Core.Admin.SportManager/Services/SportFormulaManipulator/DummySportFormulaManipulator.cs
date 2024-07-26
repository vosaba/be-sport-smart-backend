namespace Bss.Core.Admin.SportManager.Services.SportFormulaManipulator;

public class DummySportFormulaManipulator : ISportFormulaManipulator
{
    public Dictionary<string, object> GetFormulaVariables(string formula)
    {
        throw new NotImplementedException();
    }

    public string ApplyVariablesToFormula(string formula, Dictionary<string, object> variables)
    {
        throw new NotImplementedException();
    }

    public string CreateFormulaUsingData(Dictionary<string, object> variables)
    {
        throw new NotImplementedException();
    }
}