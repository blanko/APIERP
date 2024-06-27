using APIERP.Entidades;

namespace APIERP.Repositorios
{
    public interface IRepoStores
    {
        Task<int> Add(Store Store);
        Task<bool> Delete(int id);
        Task<List<Store>> GetAll();
        Task<List<Store>> GetByName(string name);
        Task<Store?> GetById(int id);
        Task Update(Store Store);
    }
}