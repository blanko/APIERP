using APIERP.DTOs;
using APIERP.Entidades;

namespace APIERP.Repositorios
{
    public interface IRepoOrders
    {
        Task<int> Add(Order Order);
        Task<List<Order>> GetAll(PaginacionDTO paginacionDTO);
        Task<List<Order>> GetAllByDate(DateTime date, PaginacionDTO paginacionDTO);
        Task<Order?> GetById(int id);
        Task<List<Order>> GetByUsername(string name);
        Task Update(Order Order);
    }
}