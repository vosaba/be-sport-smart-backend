using Bss.Api.Data.Models;

namespace Bss.Api.Data.Repositories
{
    public interface IInputRepository : IRepository
    {
        public Task<Input?> GetInput(string name);
        public Task<List<Input>> GetInputs(string[] name);
        public Task<List<Input>> GetInputsByProviderName(params string[] scoreProviderName);
    }
}
