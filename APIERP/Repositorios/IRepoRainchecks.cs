using APIERP.DTOs;
using APIERP.Entidades;

namespace APIERP.Repositorios
{
    public interface IRepoRainchecks
    {
        Task<int> Add(Raincheck Raincheck);
        Task<bool> Delete(int id);
        Task<Raincheck?> GetById(int id);
        Task Update(Raincheck RainCheck);
        Task<List<Raincheck>> GetAll(PaginacionDTO paginacionDTO);
    }
}