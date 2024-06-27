using APIERP.Entidades;
using Microsoft.EntityFrameworkCore;

namespace APIERP.Repositorios
{
    public class RepoStores : IRepoStores
    {
        private readonly ApplicationDbContext context;

        public RepoStores(ApplicationDbContext context)
        {
            this.context = context;
        }

        public async Task<Store?> GetById(int id)
        {
            return await context.Stores.FirstOrDefaultAsync(x => x.StoreId == id);
        }

        public async Task<List<Store>> GetByName(string name)
        {
            return await context.Stores.Where(x => x.Name.Contains(name))
                .OrderBy(a => a.Name).ToListAsync(); ;
        }

        public async Task<List<Store>> GetAll()
        {
            return await context.Stores.OrderBy(x => x.Name).ToListAsync();
        }

        public async Task<int> Add(Store Store)
        {
            context.Add(Store);
            await context.SaveChangesAsync();
            return Store.StoreId;
        }

        public async Task Update(Store Store)
        {
            context.Update(Store);
            await context.SaveChangesAsync();
        }

        public async Task<bool> Delete(int id)
        {
            var deletedRows = await context.Stores
             .Where(x => x.StoreId == id)
             .ExecuteDeleteAsync();

            return deletedRows > 0;
        }
    }
}
