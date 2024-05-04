using Bss.Api.Data;
using Bss.Api.Data.Models;
using Bss.Api.Data.Repositories;
using Jint;
using Microsoft.EntityFrameworkCore;

namespace Bss.Api.Services
{
    public interface IEvaluationEngine : IDisposable
    {
        public Task<double> GetScore(string formula, IDictionary<string, string> inputs);
        public void RefreshContext(params ScoreProvider[] scoreProviders);
    }

    public class EvaluationEngine : IEvaluationEngine
    {
        private readonly Engine _engine = new Engine();

        public EvaluationEngine()
        {
            _engine.Execute("var internal_context = { measures: {} };");
        }

        public void Dispose()
        {
            _engine.Dispose();
        }

        public Task<double> GetScore(string formula, IDictionary<string, string> inputs)
        {
            var inputString = "{" + string.Join(",", inputs.Select(x => $"'{x.Key}': {x.Value}")) + "}";
            var executionString = @$"(function() 
                {{ 
                    var inputs = {inputString};
                    var formula = {formula};
                    var context = {{ inputs, measures: internal_context.measures }};
                    return formula(context); 
                }})();";

            var score = _engine.Evaluate(executionString).AsNumber();

            return Task.FromResult(score);   
        }

        public void RefreshContext(params ScoreProvider[] scoreProviders)
        {
            ClearContext();

            foreach (var scoreProvider in scoreProviders)
            {
                _engine.Execute($"internal_context.measures['{scoreProvider.Name}'] = {scoreProvider.Formula};");
            }
        }

        private void ClearContext()
        {
            _engine.Execute("internal_context = { measures: {} };");
        }
    }
}
