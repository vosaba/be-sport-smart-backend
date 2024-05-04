using Bss.Api.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Bss.Api.Data
{
    public partial class BeSportSmartDbContext
    {
        public DbSet<Input> Inputs { get; set; }

        public Task<Input?> GetInput(string name)
        {
            return Inputs.SingleOrDefaultAsync(x => x.Name == name);
        }

        public Task<List<Input>> GetInputs(params string[] names)
        {
            return Inputs.Where(x => names.Contains(x.Name)).ToListAsync();
        }

        public Task<List<Input>> GetInputsByProviderName(params string[] scoreProviderNames)
        {
            return Inputs.Where(x => x.ScoreProviderInputs.Any(spi => scoreProviderNames.Contains(spi.ScoreProvider.Name))).ToListAsync();
        }

        public async Task AddInputs(params Input[] inputs)
        {
            var inputNames = inputs.Select(x => x.Name).ToList();

            var existingInputs = await Inputs
                .Where(x => inputNames.Contains(x.Name))
                .ToListAsync();

            if (existingInputs.Any())
            {
                throw new InvalidOperationException($"Some of the inputs already exists: {string.Join(",", existingInputs.Select(x => x.Name))}");
            }

            foreach (var input in inputs)
            {
                Add(input);
            }
        }
    }
}
