using APIERP.DTOs;
using APIERP.Entidades;
using APIERP.Utilidades;
using Microsoft.EntityFrameworkCore;

namespace APIERP.Repositorios
{
    public class RepoRainchecks(ApplicationDbContext context,
        IHttpContextAccessor httpContextAccessor) : IRepoRainchecks
    {
        private readonly ApplicationDbContext context = context;
        private readonly HttpContext httpContext = httpContextAccessor.HttpContext!;

        public async Task<Raincheck?> GetById(int id)
        {
            return await context.Rainchecks.FirstOrDefaultAsync(x => x.RaincheckId == id);
        }

        //public async Task<List<RainCheck>> GetByName(string name)
        //{
        //    return await context.RainChecks.Where(x => x.Title.Contains(name))
        //        .OrderBy(a => a.Title).ToListAsync(); ;
        //}

        public async Task<List<Raincheck>> GetAll(PaginacionDTO paginacionDTO)
        {
            var queryable = context.Rainchecks.AsQueryable();
            await httpContext.InsertarParametrosPaginacionEnCabecera(queryable);
            return await queryable.OrderBy(p => p.Name).Paginar(paginacionDTO).ToListAsync();
        }

        //public async Task<List<RainCheck>> GetByCategories(int id, PaginacionDTO paginacionDTO)
        //{
        //    var queryable = context.RainChecks.AsQueryable();
        //    await httpContext.InsertarParametrosPaginacionEnCabecera(queryable);
        //    return await queryable.Where(x => x.CategoryId.Equals(id))
        //        .OrderBy(p => p.Title).Paginar(paginacionDTO).ToListAsync();
        //}

        public async Task<int> Add(Raincheck RainCheck)
        {
            context.Add(RainCheck);
            await context.SaveChangesAsync();
            return RainCheck.RaincheckId;
        }

        public async Task Update(Raincheck RainCheck)
        {
            context.Update(RainCheck);
            await context.SaveChangesAsync();
        }

        public async Task<bool> Delete(int id)
        {
            var deletedRows = await context.Rainchecks
             .Where(x => x.RaincheckId == id)
             .ExecuteDeleteAsync();

            return deletedRows > 0;
        }
    }
}
