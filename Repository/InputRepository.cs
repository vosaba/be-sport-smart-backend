using api.Data;
using api.Interfaces;
using api.Models;
using Microsoft.EntityFrameworkCore;

namespace api.Repository
{
    public class InputRepository : IInputRepository
    {
        private readonly IApplicationDBContext _context;
        public InputRepository(IApplicationDBContext context)
        {
            _context = context;
        }

        public Task<Input> GetInput(string name)
        {
            return _context.Inputs.SingleAsync(x => x.Name == name);
        }

        public async Task<List<Input>> GetInputs(string scoreProviderName, bool includeDependent = true)
        {
            var allNames = new HashSet<string> { scoreProviderName };
            var newNames = new HashSet<string> { scoreProviderName };

            var scoreProviders = await _context.ScoreProviders.ToListAsync(); // Fetch all score providers and evaluate client-side


            while (newNames.Any())
            {
                var currentNames = newNames.ToList();
                newNames.Clear();
                var dependentNames = scoreProviders
                    .Where(sp => currentNames.Contains(sp.Name))
                    .SelectMany(sp => sp.DependentProviders)
                    .ToList();

                foreach (var name in dependentNames)
                {
                    if (allNames.Add(name))  // Add returns true if it was actually added (i.e., not already present)
                    {
                        newNames.Add(name);
                    }
                }
            }

            var allInputs = await _context.ScoreProviders
                .Where(sp => allNames.Contains(sp.Name))
                .SelectMany(sp => sp.ScoreProviderInputs.Select(spi => spi.Input))
                .Distinct()
                .ToListAsync();

            return allInputs;
        }

        public async Task SaveInputs(params Input[] inputs)
        {
            var existingInputs = await _context.Inputs
                .Where(x => inputs.Any(i => i.Name == x.Name))
                .ToListAsync();

            if (existingInputs.Any())
            {
                throw new InvalidOperationException($"Some of the inputs already exists: {string.Join(",", existingInputs.Select(x => x.Name))}");
            }

            await _context.Inputs.AddRangeAsync(existingInputs);
        }
    }
}
