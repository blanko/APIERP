using APIERP.DTOs;
using APIERP.Entidades;
using APIERP.Repositorios;
using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.OutputCaching;

namespace APIERP.Endpoints
{
    public static class OrdersEndpoints
    {
        public static RouteGroupBuilder MapOrders(this RouteGroupBuilder group)
        {
            group.MapGet("/", GetAll).CacheOutput(c => c.Expire(TimeSpan.FromSeconds(60)).Tag("orders-get"));
            group.MapGet("/{id:int}", GetById);
            group.MapGet("/{username}", GetByUsername);
            group.MapGet("/bydate", GetAllByDate);
            group.MapPost("/", Add);
            group.MapPut("/{id:int}", Update);
            group.MapDelete("/{id:int}", Delete);
            return group;
        }

        static async Task<Ok<List<OrderDTO>>> GetAll(IRepoOrders repo,
            IMapper mapper, int pagina = 1, int recordsPorPagina = 10)
        {
            var paginacion = new PaginacionDTO { Pagina = pagina, RecordsPorPagina = recordsPorPagina };
            var orders = await repo.GetAll(paginacion);
            var ordersDTO = mapper.Map<List<OrderDTO>>(orders);
            return TypedResults.Ok(ordersDTO);
        }

        static async Task<Results<Ok<OrderDTO>, NotFound>> GetById(int id, IRepoOrders repo,
            IMapper mapper)
        {
            var order = await repo.GetById(id);

            if (order is null) return TypedResults.NotFound();


            var orderDTO = mapper.Map<OrderDTO>(order);
            return TypedResults.Ok(orderDTO);
        }

        static async Task<Results<Ok<OrderDTO>, NotFound>> GetByUsername(string username, IRepoOrders repo,
            IMapper mapper)
        {
            var order = await repo.GetByUsername(username);

            if (order is null) return TypedResults.NotFound();


            var orderDTO = mapper.Map<OrderDTO>(order);
            return TypedResults.Ok(orderDTO);
        }

        static async Task<Ok<List<OrderDTO>>> GetAllByDate(DateTime date, IRepoOrders repo,
            IMapper mapper, int pagina = 1, int recordsPorPagina = 10)
        {
            var paginacion = new PaginacionDTO { Pagina = pagina, RecordsPorPagina = recordsPorPagina };
            var orders = await repo.GetAllByDate(date, paginacion);
            var ordersDTO = mapper.Map<List<OrderDTO>>(orders);
            return TypedResults.Ok(ordersDTO);
        }

        static async Task<Created<OrderDTO>> Add(OrderDTOAdd OrderDTOAdd,
            IRepoOrders repo, IOutputCacheStore outputCacheStore, IMapper mapper)
        {
            var order = mapper.Map<Order>(OrderDTOAdd);
            order.OrderDate = DateTime.Now;
            var id = await repo.Add(order);

            await outputCacheStore.EvictByTagAsync("orders-get", default);
            var orderDTO = mapper.Map<OrderDTO>(order);
            return TypedResults.Created($"/orders/{id}", orderDTO);
        }

        static async Task<Results<NoContent, NotFound>> Update(int id,
            OrderDTOAdd OrderDTOAdd, IRepoOrders repo,
           IOutputCacheStore outputCacheStore, IMapper mapper)
        {
            var orderDB = await repo.GetById(id);

            if (orderDB is null) return TypedResults.NotFound();


            var order = mapper.Map<Order>(OrderDTOAdd);
            order.OrderId = id;

            await repo.Update(order);
            await outputCacheStore.EvictByTagAsync("orders-get", default);
            return TypedResults.NoContent();
        }

        static async Task<Results<NoContent, NotFound>> Delete(int id,
            IOutputCacheStore outputCacheStore, IRepo<Order> repo)
        {
            var deleted = await repo.Delete(id);

            if (!deleted) return TypedResults.NotFound();

            await outputCacheStore.EvictByTagAsync("orders-get", default);
            return TypedResults.NoContent();
        }
    }
}
