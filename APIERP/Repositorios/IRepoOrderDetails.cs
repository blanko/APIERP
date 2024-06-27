using APIERP.Entidades;

namespace APIERP.Repositorios
{
    public interface IRepoOrderDetails
    {
        Task<int> Add(OrderDetail OrderDetail);
        Task<List<OrderDetail>> GetAllByOrderID(int orderId);
        Task<OrderDetail?> GetById(int id);
        Task Update(OrderDetail OrderDetail);
    }
}