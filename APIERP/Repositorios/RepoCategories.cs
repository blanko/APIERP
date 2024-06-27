using Microsoft.EntityFrameworkCore;
using APIERP.Entidades;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace APIERP.Repositorios
{
    public class RepoCategories : IRepoCategories
    {
        private readonly ApplicationDbContext context;

        public RepoCategories(ApplicationDbContext context)
        {
            this.context = context;
        }

        public async Task<bool> Exist(int id)
        {
            return await context.Categories.AnyAsync(x => x.CategoryId == id);
        }

        public async Task<bool> Exist(int id, string name)
        {
            return await context.Categories.AnyAsync(g => g.CategoryId != id && g.Name == name);
        }

        public async Task<List<int>> Existen(List<int> ids)
        {
            return await context.Categories.Where(g => ids.Contains(g.CategoryId))
                .Select(g => g.CategoryId).ToListAsync();
        }

        public async Task<Category?> GetById(int id)
        {
            return await context.Categories.FirstOrDefaultAsync(x => x.CategoryId == id);
        }

        public async Task<List<Category>> GetByName(string name)
        {
            return await context.Categories.Where(x => x.Name.Contains(name))
                .OrderBy(a => a.Name).ToListAsync(); ;
        }

        public async Task<List<Category>> GetAll()
        {
            return await context.Categories.OrderBy(x => x.Name).ToListAsync();
        }

        public async Task<int> Add(Category Category)
        {
            context.Add(Category);
            await context.SaveChangesAsync();
            return Category.CategoryId;
        }

        public async Task Update(Category Category)
        {
            context.Update(Category);
            await context.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            await context.Categories.Where(x => x.CategoryId == id).ExecuteDeleteAsync();
        }
    }
}
