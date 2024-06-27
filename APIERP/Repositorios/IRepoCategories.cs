
using APIERP.Entidades;

namespace APIERP.Repositorios
{
    public interface IRepoCategories
    {
        Task Update(Category Category);
        Task Delete(int id);
        Task<int> Add(Category Category);
        Task<bool> Exist(int id);
        Task<bool> Exist(int id, string name);
        Task<List<int>> Existen(List<int> ids);
        Task<Category?> GetById(int id);
        Task<List<Category>> GetAll();
        Task<List<Category>> GetByName(string name);
    }
}