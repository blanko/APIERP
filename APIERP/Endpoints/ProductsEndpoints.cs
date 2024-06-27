using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using APIERP.DTOs;
using APIERP.Repositorios;
using APIERP.Entidades;

namespace APIERP.Endpoints
{
    public static class ProductsEndpoints
    {

        public static RouteGroupBuilder MapProducts(this RouteGroupBuilder group)
        {
            group.MapGet("/", GetAll).CacheOutput(c => c.Expire(TimeSpan.FromSeconds(60)).Tag("products-get"));
            group.MapGet("/{id:int}", GetById);
            group.MapGet("/bycategory/{id:int}", GetByCategories);
            group.MapPost("/", Add);
            group.MapPut("/{id:int}", Update);
            group.MapDelete("/{id:int}", Delete);
            return group;
        }

        static async Task<Ok<List<ProductDTO>>> GetAll(IRepoProducts repo,
            IMapper mapper, int pagina = 1, int recordsPorPagina = 10)
        {
            var paginacion = new PaginacionDTO { Pagina = pagina, RecordsPorPagina = recordsPorPagina };
            var products = await repo.GetAll(paginacion);
            var productsDTO = mapper.Map<List<ProductDTO>>(products);
            return TypedResults.Ok(productsDTO);
        }

        static async Task<Results<Ok<ProductDTO>, NotFound>> GetById(int id, IRepoProducts repo,
            IMapper mapper)
        {
            var product = await repo.GetById(id);

            if (product is null) return TypedResults.NotFound();


            var productDTO = mapper.Map<ProductDTO>(product);
            return TypedResults.Ok(productDTO);
        }

        static async Task<Ok<List<ProductDTO>>> GetByCategories(int id, IRepoProducts repo,
            IMapper mapper, int pagina = 1, int recordsPorPagina = 10)
        {
            var paginacion = new PaginacionDTO { Pagina = pagina, RecordsPorPagina = recordsPorPagina };
            var products = await repo.GetByCategories(id, paginacion);
            var productsDTO = mapper.Map<List<ProductDTO>>(products);
            return TypedResults.Ok(productsDTO);
        }

        static async Task<Created<ProductDTO>> Add(ProductDTOAdd ProductDTOAdd,
            IRepoProducts repo, IOutputCacheStore outputCacheStore, IMapper mapper)
        {
            var product = mapper.Map<Product>(ProductDTOAdd);
            product.Created = DateTime.Now;
            var id = await repo.Add(product);

            await outputCacheStore.EvictByTagAsync("products-get", default);
            var productDTO = mapper.Map<ProductDTO>(product);
            return TypedResults.Created($"/products/{id}", productDTO);
        }

        static async Task<Results<NoContent, NotFound>> Update(int id,
            ProductDTOAdd ProductDTOAdd, IRepoProducts repo,
           IOutputCacheStore outputCacheStore, IMapper mapper)
        {
            var productDB = await repo.GetById(id);

            if (productDB is null) return TypedResults.NotFound();


            var product = mapper.Map<Product>(ProductDTOAdd);
            product.ProductId = id;

            await repo.Update(product);
            await outputCacheStore.EvictByTagAsync("products-get", default);
            return TypedResults.NoContent();
        }

        static async Task<Results<NoContent, NotFound>> Delete(int id,
            IOutputCacheStore outputCacheStore, IRepo<Product> repo)
        {
            var deleted = await repo.Delete(id);

            if (!deleted) return TypedResults.NotFound();

            await outputCacheStore.EvictByTagAsync("products-get", default);
            return TypedResults.NoContent();
        }
    }
}
