using APIERP.DTOs;
using APIERP.Entidades;
using APIERP.Repositorios;
using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.OutputCaching;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace APIERP.Endpoints
{
    public static class RainChecksEndpoints
    {
        public static RouteGroupBuilder MapRainChecks(this RouteGroupBuilder group)
        {
            group.MapGet("/", GetAll).CacheOutput(c => c.Expire(TimeSpan.FromSeconds(60)).Tag("rainchecks-get"));
            group.MapGet("/{id:int}", GetById);
            group.MapPost("/", Add);
            group.MapPut("/{id:int}", Update);
            group.MapDelete("/{id:int}", Delete);
            return group;
        }

        static async Task<Ok<List<RaincheckDTO>>> GetAll(IRepoRainchecks repo,
            IMapper mapper, int pagina = 1, int recordsPorPagina = 10)
        {
            var paginacion = new PaginacionDTO { Pagina = pagina, RecordsPorPagina = recordsPorPagina };
            var rainChecks = await repo.GetAll(paginacion);
            var rainChecksDTO = mapper.Map<List<RaincheckDTO>>(rainChecks);
            return TypedResults.Ok(rainChecksDTO);
        }

        static async Task<Results<Ok<RaincheckDTO>, NotFound>> GetById(int id, IRepoRainchecks repo,
            IMapper mapper)
        {
            var rainCheck = await repo.GetById(id);

            if (rainCheck is null) return TypedResults.NotFound();


            var rainCheckDTO = mapper.Map<RaincheckDTO>(rainCheck);
            return TypedResults.Ok(rainCheckDTO);
        }

        // Pendiente de las modificiaciones y pasarlo al contexto
        //static async Task<Results<Ok<List<RaincheckDTOCompleto>>, NotFound>> GetRainCheckCompleto
        //    (ApplicationDbContext db, int pagina = 1, int recordsPorPagina = 10)
        //{
        //    var data = await db.Rainchecks
        //        .OrderBy(s => s.RaincheckId)
        //        .Skip(pagina * recordsPorPagina)
        //        .Take(recordsPorPagina)
        //        .Include(s => s.Product)
        //        .Include(s => s.Product.Category)
        //        .Include(s => s.Store)
        //        .Select(x => new RaincheckDTO
        //        {
        //            Name = x.Name,
        //            Count = x.Count,
        //            SalePrice = x.SalePrice,
        //            Store = new StoreDto
        //            {
        //                Name = x.Store.Name
        //            },
        //            Product = new ProductDto
        //            {
        //                Name = x.Product.Title,
        //                Category = new CategoryDto
        //                {
        //                    Name = x.Product.Category.Name
        //                }
        //            }
        //        })
        //        .ToListAsync();

        //    return data.Any()
        //        ? TypedResults.Ok(data)
        //        : TypedResults.NotFound();
        //}

        static async Task<Created<RaincheckDTO>> Add(RaincheckDTOAdd RaincheckDTOAdd,
            IRepoRainchecks repo, IOutputCacheStore outputCacheStore, IMapper mapper)
        {
            var rainCheck = mapper.Map<Raincheck>(RaincheckDTOAdd);
            var id = await repo.Add(rainCheck);

            await outputCacheStore.EvictByTagAsync("rainchecks-get", default);
            var rainCheckDTO = mapper.Map<RaincheckDTO>(rainCheck);
            return TypedResults.Created($"/rainchecks/{id}", rainCheckDTO);
        }

        static async Task<Results<NoContent, NotFound>> Update(int id,
            RaincheckDTOAdd RaincheckDTOAdd, IRepoRainchecks repo,
           IOutputCacheStore outputCacheStore, IMapper mapper)
        {
            var rainCheckDB = await repo.GetById(id);

            if (rainCheckDB is null) return TypedResults.NotFound();


            var rainCheck = mapper.Map<Raincheck>(RaincheckDTOAdd);
            rainCheck.RaincheckId = id;

            await repo.Update(rainCheck);
            await outputCacheStore.EvictByTagAsync("rainchecks-get", default);
            return TypedResults.NoContent();
        }

        static async Task<Results<NoContent, NotFound>> Delete(int id,
            IOutputCacheStore outputCacheStore, IRepo<Raincheck> repo)
        {
            var deleted = await repo.Delete(id);

            if (!deleted) return TypedResults.NotFound();

            await outputCacheStore.EvictByTagAsync("rainchecks-get", default);
            return TypedResults.NoContent();
        }
    }
}
