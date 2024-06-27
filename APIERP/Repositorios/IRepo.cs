
namespace APIERP.Repositorios
{
    public interface IRepo<T> where T : class
    {
        Task<bool> Add(T entity);
        Task<bool> Delete(int id);
        Task<IEnumerable<T>> GetAll();
        Task<T?> GetById(int id);
        Task<bool> Update(T entity);
    }
}