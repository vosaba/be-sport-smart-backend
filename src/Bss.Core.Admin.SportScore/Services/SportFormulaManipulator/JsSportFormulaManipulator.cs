using Bss.Core.Bl.Models;
using Bss.Core.Bl.Services.ComputationAnalyzers;
using System.Text.RegularExpressions;

namespace Bss.Core.Admin.SportScore.Services.SportFormulaManipulator;

public class JsSportFormulaManipulator : ISportFormulaManipulator
{
    public Dictionary<string, double> GetSportScoreData(Computation computation)
    {
        return ExtractVariablesToDictionary(computation.Formula);
    }

    public async Task ApplyScoreDataToFormula(Computation computation, Dictionary<string, double> scoreData)
    {
        var newFormula = ApplyDictionaryToFormula(computation.Formula, scoreData);

        await computation.SetFormula(
            newFormula,
            computation => Task.FromResult(new ComputationRequirements(
                [.. computation.RequiredComputations],
                [.. computation.RequiredMeasures])),
            computation => Task.CompletedTask);
    }

    private static Dictionary<string, double> ExtractVariablesToDictionary(string input)
    {
        var dict = new Dictionary<string, double>();
        var simpleVarPattern = @"const\s+(?<name>\w+)\s*=\s*(?<value>\d+(\.\d+)?);";
        var nestedVarPattern = @"const\s+(?<name>\w+)\s*=\s*\{(?<content>.*?)\};";

        var variableMatches = Regex.Matches(input, simpleVarPattern);
        foreach (Match match in variableMatches)
        {
            var name = match.Groups["name"].Value;
            var value = double.Parse(match.Groups["value"].Value);
            dict.Add(name, value);
        }

        var objectMatches = Regex.Matches(input, nestedVarPattern, RegexOptions.Singleline);
        foreach (Match match in objectMatches)
        {
            var name = match.Groups["name"].Value;
            var content = match.Groups["content"].Value;

            var nestedVariablePattern = @"(?<key>\w+)\s*:\s*\{\s*(?<innerContent>[^}]+)\s*\}";
            var nestedVariableMatches = Regex.Matches(content, nestedVariablePattern);
            foreach (Match nestedMatch in nestedVariableMatches)
            {
                var key = nestedMatch.Groups["key"].Value;
                var innerContent = nestedMatch.Groups["innerContent"].Value;
                var innerPattern = @"(?<subkey>\w+)\s*:\s*(?<subvalue>\d+(\.\d+)?)";
                var innerMatches = Regex.Matches(innerContent, innerPattern);
                foreach (Match innerMatch in innerMatches)
                {
                    var subkey = innerMatch.Groups["subkey"].Value;
                    var subvalue = double.Parse(innerMatch.Groups["subvalue"].Value);
                    dict.Add($"{name}.{key}.{subkey}", subvalue);
                }
            }
        }

        return dict;
    }

    private static string ApplyDictionaryToFormula(string formula, Dictionary<string, double> dictionary)
    {
        foreach (var entry in dictionary)
        {
            string pattern;
            if (entry.Key.Contains('.'))
            {
                var parts = entry.Key.Split('.');
                var nestedKey = parts[0];
                var subKey = parts[1];
                var subSubKey = parts[2];
                pattern = $@"const\s+{nestedKey}\s*=\s*\{{[^\}}]*{subKey}\s*:\s*\{{[^\}}]*{subSubKey}\s*:\s*\d+(\.\d+)?";
                formula = Regex.Replace(formula, pattern, match =>
                {
                    var nestedPattern = $@"{subSubKey}\s*:\s*\d+(\.\d+)?";
                    return Regex.Replace(match.Value, nestedPattern, $"{subSubKey}: {entry.Value}");
                });
            }
            else
            {
                pattern = $@"const\s+{entry.Key}\s*=\s*\d+(\.\d+)?;";
                formula = Regex.Replace(formula, pattern, $"const {entry.Key} = {entry.Value};");
            }
        }

        return formula;
    }
}
