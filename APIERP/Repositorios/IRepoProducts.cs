using APIERP.DTOs;
using APIERP.Entidades;

namespace APIERP.Repositorios
{
    public interface IRepoProducts
    {
        Task<int> Add(Product Store);
        Task<List<Product>> GetAll(PaginacionDTO paginacionDTO);
        Task<List<Product>> GetByCategories(int id, PaginacionDTO paginacionDTO);
        Task<Product?> GetById(int id);
        Task<List<Product>> GetByName(string name);
        Task Update(Product Product);
        Task<bool> Delete(int id);
    }
}