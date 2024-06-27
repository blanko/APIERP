using APIERP.DTOs;
using APIERP.Entidades;
using APIERP.Repositorios;
using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.OutputCaching;

namespace APIERP.Endpoints
{
    public static class OrderDetailsEndpoints
    {
        public static RouteGroupBuilder MapOrderDetails(this RouteGroupBuilder group)
        {
            group.MapGet("/{id:int}", GetById);
            group.MapGet("/order/{orderId:int}", GetAllByOrderID);
            group.MapPost("/", Add);
            group.MapPut("/{id:int}", Update);
            group.MapDelete("/{id:int}", Delete);
            return group;
        }

        static async Task<Results<Ok<OrderDetailDTO>, NotFound>> GetById(int id, IRepoOrderDetails repo,
            IMapper mapper)
        {
            var orderDetail = await repo.GetById(id);

            if (orderDetail is null) return TypedResults.NotFound();


            var orderDetailDTO = mapper.Map<OrderDetailDTO>(orderDetail);
            return TypedResults.Ok(orderDetailDTO);
        }

        static async Task<Ok<List<OrderDetailDTO>>> GetAllByOrderID(int orderId, IRepoOrderDetails repo,
            IMapper mapper)
        {
            var orderDetails = await repo.GetAllByOrderID(orderId);
            var orderDetailsDTO = mapper.Map<List<OrderDetailDTO>>(orderDetails);
            return TypedResults.Ok(orderDetailsDTO);
        }

        static async Task<Created<OrderDetailDTO>> Add(OrderDetailDTOAdd OrderDetailDTOAdd,
            IRepoOrderDetails repo, IOutputCacheStore outputCacheStore, IMapper mapper)
        {
            var orderDetail = mapper.Map<OrderDetail>(OrderDetailDTOAdd);
            var id = await repo.Add(orderDetail);

            await outputCacheStore.EvictByTagAsync("orderDetails-get", default);
            var orderDetailDTO = mapper.Map<OrderDetailDTO>(orderDetail);
            return TypedResults.Created($"/orderDetails/{id}", orderDetailDTO);
        }

        static async Task<Results<NoContent, NotFound>> Update(int id,
            OrderDetailDTOAdd OrderDetailDTOAdd, IRepoOrderDetails repo,
           IOutputCacheStore outputCacheStore, IMapper mapper)
        {
            var orderDetailDB = await repo.GetById(id);

            if (orderDetailDB is null) return TypedResults.NotFound();


            var orderDetail = mapper.Map<OrderDetail>(OrderDetailDTOAdd);
            orderDetail.OrderDetailId = id;

            await repo.Update(orderDetail);
            await outputCacheStore.EvictByTagAsync("orderDetails-get", default);
            return TypedResults.NoContent();
        }

        static async Task<Results<NoContent, NotFound>> Delete(int id,
            IOutputCacheStore outputCacheStore, IRepo<OrderDetail> repo)
        {
            var deleted = await repo.Delete(id);

            if (!deleted) return TypedResults.NotFound();

            await outputCacheStore.EvictByTagAsync("orderDetails-get", default);
            return TypedResults.NoContent();
        }
    }
}
