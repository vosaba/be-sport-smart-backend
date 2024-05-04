namespace Bss.Api.Data.Repositories
{
    public interface IRepository
    {
        void Add<T>(T aggregationRoot);

        void Remove<T>(T aggregationRoot);

        Task ApplyChanges();
    }
}