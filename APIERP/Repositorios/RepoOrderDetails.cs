using APIERP.DTOs;
using APIERP.Entidades;
using APIERP.Utilidades;
using Microsoft.EntityFrameworkCore;

namespace APIERP.Repositorios
{
    public class RepoOrderDetails(ApplicationDbContext context) : IRepoOrderDetails
    {
        private readonly ApplicationDbContext context = context;

        public async Task<OrderDetail?> GetById(int id)
        {
            return await context.OrderDetails.FirstOrDefaultAsync(x => x.OrderDetailId == id);
        }

        public async Task<List<OrderDetail>> GetAllByOrderID(int orderId)
        {
            return await context.OrderDetails.Where(x => x.OrderId.Equals(orderId))
                .OrderBy(p => p.OrderDetailId).ToListAsync();
        }

        public async Task<int> Add(OrderDetail OrderDetail)
        {
            context.Add(OrderDetail);
            await context.SaveChangesAsync();
            return OrderDetail.OrderDetailId;
        }

        public async Task Update(OrderDetail OrderDetail)
        {
            context.Update(OrderDetail);
            await context.SaveChangesAsync();
        }
    }
}
