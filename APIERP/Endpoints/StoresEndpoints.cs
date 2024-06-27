using APIERP.DTOs;
using APIERP.Entidades;
using APIERP.Repositorios;
using APIERP.Servicios;
using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.OutputCaching;
using System.ComponentModel;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace APIERP.Endpoints
{
    public static class StoresEndpoints
    {
        public static RouteGroupBuilder MapStores(this RouteGroupBuilder group)
        {
            group.MapGet("/", GetAll).CacheOutput(c => c.Expire(TimeSpan.FromSeconds(60)).Tag("stores-get"));
            group.MapGet("/{id:int}", GetById);
            group.MapGet("/{name}", GetByName);
            //group.MapPost("/", Add).AddEndpointFilter<FiltroValidaciones<CategoryDTOAdd>>();
            group.MapPost("/", Add);
            //group.MapPut("/{id:int}", Update).AddEndpointFilter<FiltroValidaciones<CategoryDTOAdd>>();
            group.MapPut("/{id:int}", Update);
            group.MapDelete("/{id:int}", Delete);
            return group;
        }

        static async Task<Ok<List<StoreDTO>>> GetAll(IRepoStores repo, IMapper mapper)
        {
            var stores = await repo.GetAll();
            var storesDTO = mapper.Map<List<StoreDTO>>(stores);
            return TypedResults.Ok(storesDTO);
        }

        static async Task<Results<Ok<StoreDTO>, NotFound>> GetById(int id, IRepoStores repo,
            IMapper mapper)
        {
            var store = await repo.GetById(id);

            if (store is null)
            {
                return TypedResults.NotFound();
            }

            var StoreDTO = mapper.Map<StoreDTO>(store);

            return TypedResults.Ok(StoreDTO);
        }

        static async Task<Ok<List<StoreDTO>>> GetByName(string name,
           IRepoStores repo, IMapper mapper)
        {
            var stores = await repo.GetByName(name);
            var storesDTO = mapper.Map<List<StoreDTO>>(stores);
            return TypedResults.Ok(storesDTO);
        }

        static async Task<Results<Created<StoreDTO>, ValidationProblem>> Add(
            StoreDTOAdd StoreDTOAdd, IRepoStores repo,
            IOutputCacheStore outputCacheStore, IMapper mapper)
        {
            var store = mapper.Map<Store>(StoreDTOAdd);
            var id = await repo.Add(store);
            await outputCacheStore.EvictByTagAsync("stores-get", default);
            var StoreDTO = mapper.Map<StoreDTO>(store);
            return TypedResults.Created($"/stores/{id}", StoreDTO);
        }

        static async Task<Results<NoContent, NotFound, ValidationProblem>> Update(int id,
            StoreDTOAdd StoreDTOAdd, IRepoStores repo,
            IOutputCacheStore outputCacheStore, IMapper mapper)
        {
            var storeDB = await repo.GetById(id);

            if (storeDB is null)
            {
                return TypedResults.NotFound();
            }

            var store = mapper.Map<Store>(StoreDTOAdd);
            store.StoreId = id;

            await repo.Update(store);
            await outputCacheStore.EvictByTagAsync("stores-get", default);
            return TypedResults.NoContent();
        }

        static async Task<Results<NoContent, NotFound>> Delete(int id, IRepoStores repo,
            IOutputCacheStore outputCacheStore)
        {
            var deleted = await repo.Delete(id);

            if (!deleted) return TypedResults.NotFound();

            await outputCacheStore.EvictByTagAsync("stores-get", default);
            return TypedResults.NoContent();
        }
    }
}
