using APIERP.DTOs;
using APIERP.Entidades;
using APIERP.Utilidades;
using Microsoft.EntityFrameworkCore;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace APIERP.Repositorios
{
    public class RepoOrders(ApplicationDbContext context,
        IHttpContextAccessor httpContextAccessor) : IRepoOrders
    {
        private readonly ApplicationDbContext context = context;
        private readonly HttpContext httpContext = httpContextAccessor.HttpContext!;

        public async Task<Order?> GetById(int id)
        {
            return await context.Orders.FirstOrDefaultAsync(x => x.OrderId == id);
        }

        public async Task<List<Order>> GetByUsername(string username)
        {
            return await context.Orders.Where(x => x.Username.Equals(username))
                .OrderBy(a => a.OrderDate).ToListAsync(); ;
        }

        public async Task<List<Order>> GetAll(PaginacionDTO paginacionDTO)
        {
            var queryable = context.Orders.AsQueryable();
            await httpContext.InsertarParametrosPaginacionEnCabecera(queryable);
            return await queryable.OrderBy(p => p.OrderId).Paginar(paginacionDTO).ToListAsync();
        }

        public async Task<List<Order>> GetAllByDate(DateTime date, PaginacionDTO paginacionDTO)
        {
            var queryable = context.Orders.AsQueryable();
            await httpContext.InsertarParametrosPaginacionEnCabecera(queryable);
            return await queryable.Where(x => x.OrderDate.Date == date.Date)
                .OrderBy(p => p.OrderDate).Paginar(paginacionDTO).ToListAsync();
        }

        public async Task<int> Add(Order Order)
        {
            context.Add(Order);
            await context.SaveChangesAsync();
            return Order.OrderId;
        }

        public async Task Update(Order Order)
        {
            context.Update(Order);
            await context.SaveChangesAsync();
        }
    }
}
