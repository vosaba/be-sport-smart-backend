using Bss.Core.Admin.SportManager.Configurations;
using Bss.Core.Bl.Data;
using Bss.Core.Bl.Enums;
using Bss.Core.Bl.Models;
using Bss.Infrastructure.Errors.Abstractions;
using Esprima;
using Esprima.Ast;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Bss.Core.Admin.SportManager.Services.SportFormulaManipulator;

public class JsSportFormulaManipulator(
    ICoreDbContext coreDbContext,
    IOptions<BssCoreAdminSportManagerConfiguration> configuration) 
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
                string stringValue => $"\"{stringValue}\"",
                double doubleValue => doubleValue.ToString(),
                int intValue => intValue.ToString(),
                long longValue => longValue.ToString(),
                bool boolValue => boolValue.ToString().ToLower(),
                _ => throw new InvalidOperationException($"Unsupported type for variable {variableName}"),
            };

            formula = formula.Remove(start, end - start);
            formula = formula.Insert(start, variableValueString);
        }

        return formula;
    }

    public string CreateFormulaUsingData(Dictionary<string, object> variables)
    {
        if (!configuration.Value.SportFormulaTemplateNames.TryGetValue(ComputationEngine.Js, out string? sportFormulaTemplateName))
        {
            throw new OperationException("Js SportFormulaTemplateName is not set in configuration");
        }

        var sportFormulaTemplate = coreDbContext.Computations
            .AsNoTracking()
            .FirstOrDefault(x => x.Name == sportFormulaTemplateName && x.Engine == ComputationEngine.Js)
            ?? throw new NotFoundException(sportFormulaTemplateName, nameof(Computation));

        return ApplyVariablesToFormula(sportFormulaTemplate.Formula, variables);
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
                        ProcessNode(declaration.Init, variableName);
                    }
                }
            }

            foreach (var child in node.ChildNodes)
            {
                Traverse(child, prefix);
            }
        }

        void ProcessNode(Node node, string prefix)
        {
            if (node is Literal literal)
            {
                callback(prefix, literal);
            }
            else if (node is ObjectExpression objectExpression)
            {
                ExtractObjectProperties(objectExpression, prefix);
            }
            else if (node is ArrayExpression arrayExpression)
            {
                ExtractArrayElements(arrayExpression, prefix);
            }
        }

        void ExtractObjectProperties(ObjectExpression obj, string prefix)
        {
            foreach (var property in obj.Properties)
            {
                if (property is Property prop)
                {
                    var propertyName = prefix + "." + ((prop.Key as Identifier)?.Name ?? prop.Key.ToString());
                    ProcessNode(prop.Value, propertyName);
                }
            }
        }

        void ExtractArrayElements(ArrayExpression array, string prefix)
        {
            for (int i = 0; i < array.Elements.Count; i++)
            {
                var element = array.Elements[i];
                var elementName = $"{prefix}[{i}]";
                ProcessNode(element, elementName);
            }
        }

        Traverse(script, string.Empty);
    }
}
