using Bss.Api.Data;
using Bss.Api.Data.Models;
using Bss.Api.Data.Repositories;
using Jint;
using Microsoft.EntityFrameworkCore;

namespace Bss.Api.Services
{
    public interface IEvaluationEngine : IDisposable
    {
        public bool Initialized { get; }
        public Task<double> GetScore(string formula, IDictionary<string, string> inputs);
        public void RefreshContext(ICollection<ScoreProvider> measures, ICollection<ScoreProvider> scores);
    }

    public class EvaluationEngine : IEvaluationEngine
    {
        private readonly Engine _engine = new Engine();
        public bool Initialized { get; private set; }

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
                    var context = {{ inputs, measures: internal_context.measures, scores: internal_context.scores }};
                    return formula(context); 
                }})();";

            var score = _engine.Evaluate(executionString).AsNumber();

            return Task.FromResult(score);   
        }

        public void RefreshContext(ICollection<ScoreProvider> measures, ICollection<ScoreProvider> scores)
        {
            ClearContext();

            foreach (var scoreProvider in measures)
            {
                _engine.Execute($"internal_context.measures['{scoreProvider.Name}'] = {scoreProvider.Formula};");
            }

            foreach (var scoreProvider in scores)
            {
                _engine.Execute($"internal_context.scores['{scoreProvider.Name}'] = {scoreProvider.Formula};");
            }

            Initialized = true;
        }

        private void ClearContext()
        {
            if (!Initialized)
            {
                _engine.Execute("var internal_context = { measures: {}, scores: {} };");
            }
            else
            {
                _engine.Execute("internal_context = { measures: {}, scores: {} };");
            }
        }
    }
}
