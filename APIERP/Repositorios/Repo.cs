using Microsoft.EntityFrameworkCore;

namespace APIERP.Repositorios
{
    public class Repo<T>(ApplicationDbContext context) : IRepo<T> where T : class
    {
        private readonly ApplicationDbContext context = context;

        public async Task<bool> Delete(int id)
        {
            var entity = await context.Set<T>().FindAsync(id);

            if (entity == null) return false;


            context.Set<T>().Remove(entity);
            var deletedRows = await context.SaveChangesAsync();

            return deletedRows > 0;
        }

        // Pendientes de implementar y usar
        public async Task<T?> GetById(int id)
        {
            return await context.Set<T>().FindAsync(id);
        }

        public async Task<IEnumerable<T>> GetAll()
        {
            return await context.Set<T>().ToListAsync();
        }

        public async Task<bool> Add(T entity)
        {
            context.Set<T>().Add(entity);
            var addedRows = await context.SaveChangesAsync();
            return addedRows > 0;
        }

        public async Task<bool> Update(T entity)
        {
            context.Set<T>().Update(entity);
            var updatedRows = await context.SaveChangesAsync();
            return updatedRows > 0;
        }
    }
}
