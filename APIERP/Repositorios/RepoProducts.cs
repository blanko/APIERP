using APIERP.DTOs;
using APIERP.Entidades;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Net.Http;
using APIERP.Utilidades;

namespace APIERP.Repositorios
{
    public class RepoProducts(ApplicationDbContext context,
        IHttpContextAccessor httpContextAccessor) : IRepoProducts
    {
        private readonly ApplicationDbContext context = context;
        private readonly HttpContext httpContext = httpContextAccessor.HttpContext!;

        public async Task<Product?> GetById(int id)
        {
            return await context.Products.FirstOrDefaultAsync(x => x.ProductId == id);
        }

        public async Task<List<Product>> GetByName(string name)
        {
            return await context.Products.Where(x => x.Title.Contains(name))
                .OrderBy(a => a.Title).ToListAsync(); ;
        }

        public async Task<List<Product>> GetAll(PaginacionDTO paginacionDTO)
        {
            var queryable = context.Products.AsQueryable();
            await httpContext.InsertarParametrosPaginacionEnCabecera(queryable);
            return await queryable.OrderBy(p => p.Title).Paginar(paginacionDTO).ToListAsync();
        }
        
        public async Task<List<Product>> GetByCategories(int id, PaginacionDTO paginacionDTO)
        {
            var queryable = context.Products.AsQueryable();
            await httpContext.InsertarParametrosPaginacionEnCabecera(queryable);
            return await queryable.Where(x => x.CategoryId.Equals(id))
                .OrderBy(p => p.Title).Paginar(paginacionDTO).ToListAsync();
        }

        public async Task<int> Add(Product Product)
        {
            context.Add(Product);
            await context.SaveChangesAsync();
            return Product.ProductId;
        }

        public async Task Update(Product Product)
        {
            context.Update(Product);
            await context.SaveChangesAsync();
        }

        public async Task<bool> Delete(int id)
        {
            var deletedRows = await context.Products
             .Where(x => x.ProductId == id)
             .ExecuteDeleteAsync();

            return deletedRows > 0;
        }
    }
}
