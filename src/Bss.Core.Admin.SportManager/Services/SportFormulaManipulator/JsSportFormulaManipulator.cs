using Bss.Core.Admin.SportManager.Configurations;
using Esprima;
using Esprima.Ast;
using Microsoft.Extensions.Options;

namespace Bss.Core.Admin.SportManager.Services.SportFormulaManipulator;

public class JsSportFormulaManipulator(IOptions<BssCoreAdminSportManagerConfiguration> options) 
    : ISportFormulaManipulator
{
    public Dictionary<string, object> GetFormulaVariables(string formula)
    {
        var variables = new Dictionary<string, object>();

        WalkOverVariableLiterals(formula, (variableName, literal) => variables.Add(variableName, literal.Value));

        return variables;
    }

    public string ApplyVariablesToFormula(string formula, Dictionary<string, object> variables)
    {
        var variablePositionsStack= new Stack<(string, (int start, int end))>();

        WalkOverVariableLiterals(formula, (variableName, literal) =>
        {
            if (variables.ContainsKey(variableName))
            {
                variablePositionsStack.Push((variableName, (literal.Range.Start, literal.Range.End)));
            }
        });

        while (variablePositionsStack.Count > 0)
        {
            var (variableName, (start, end)) = variablePositionsStack.Pop();
            var variableValue = variables[variableName];
            var variableValueString = variableValue switch
            {
                string stringValue => stringValue,
                double doubleValue => doubleValue.ToString(),
                int intValue => intValue.ToString(),
                long longValue => longValue.ToString(),
                bool boolValue => boolValue.ToString(),
                _ => throw new InvalidOperationException($"Unsupported type for variable {variableName}"),
            };

            formula = formula.Remove(start, end - start);
            formula = formula.Insert(start, variableValueString);
        }

        return formula;
    }

    public string CreateFormulaUsingData(Dictionary<string, object> variables)
    {
        var formula = options.Value.JsSportFormulaTemplate;

        if (string.IsNullOrWhiteSpace(formula))
        {
            throw new InvalidOperationException("JsSportFormulaTemplate is not set in configuration");
        }

        return ApplyVariablesToFormula(formula, variables);
    }

    private static void WalkOverVariableLiterals(string formula, Action<string, Literal> callback)
    {
        var parser = new JavaScriptParser();
        var script = parser.ParseScript(formula);

        void Traverse(Node node, string prefix)
        {
            if (node is VariableDeclaration variableDeclaration)
            {
                foreach (var declaration in variableDeclaration.Declarations)
                {
                    if (declaration.Id is Identifier identifier)
                    {
                        var variableName = prefix + identifier.Name;

                        if (declaration.Init is Literal literal)
                        {
                            callback(variableName, literal);
                        }
                        else if (declaration.Init is ObjectExpression objectExpression)
                        {
                            ExtractObjectProperties(objectExpression, variableName);
                        }
                    }
                }
            }

            foreach (var child in node.ChildNodes)
            {
                Traverse(child, prefix);
            }
        }

        void ExtractObjectProperties(ObjectExpression obj, string prefix)
        {
            foreach (var property in obj.Properties)
            {
                if (property is Property prop)
                {
                    var propertyName = prefix + "." + (prop.Key as Identifier)?.Name;

                    if (prop.Value is Literal literal)
                    {
                        callback(propertyName, literal);
                    }
                    else if (prop.Value is ObjectExpression nestedObject)
                    {
                        ExtractObjectProperties(nestedObject, propertyName);
                    }
                }
            }
        }

        Traverse(script, string.Empty);
    }
}
