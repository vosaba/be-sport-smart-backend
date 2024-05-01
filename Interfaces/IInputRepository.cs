using api.Models;

namespace api.Interfaces
{
    public interface IInputRepository
    {
        public Task<Input> GetInput(string name);
        public Task<List<Input>> GetInputs(string scoreProviderName, bool includeDependent = true);
        public Task SaveInputs(params Input[] inputs);
    }
}