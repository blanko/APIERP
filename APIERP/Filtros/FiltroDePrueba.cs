﻿
using AutoMapper;
using APIERP.Repositorios;

namespace APIERP.Filtros
{
    public class FiltroDePrueba : IEndpointFilter
    {
        public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, 
            EndpointFilterDelegate next)
        {
            // Este código se ejecuta antes del endpoint

            var paramRepositorioGeneros = context.Arguments.OfType<IRepoCategories>().FirstOrDefault();
            var paramEntero = context.Arguments.OfType<int>().FirstOrDefault();
            var paramMapper = context.Arguments.OfType<IMapper>().FirstOrDefault();

            var resultado = await next(context);
            // Este código se ejecuta después del endpoint
            return resultado;
        }
    }
}