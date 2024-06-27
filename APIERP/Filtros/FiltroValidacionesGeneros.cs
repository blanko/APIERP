
using FluentValidation;
using APIERP.DTOs;

namespace APIERP.Filtros
{
    public class FiltroValidacionesGeneros : IEndpointFilter
    {
        public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
        {
            //@TODO Terminar en el futuro, hoy no da tiempo


            //var validador = context.HttpContext.RequestServices.GetService<IValidator<CategoriasDTOCrear>>();

            //if (validador is null)
            //{
            //    return await next(context);
            //}

            //var insumoAValidar = context.Arguments.OfType<CategoriasDTOCrear>().FirstOrDefault();

            //if (insumoAValidar is null)
            //{
            //    return TypedResults.Problem("No pudo ser encontrada la entidad a validar");
            //}

            //var resultadoValidacion = await validador.ValidateAsync(insumoAValidar);

            //if (!resultadoValidacion.IsValid)
            //{
            //    return TypedResults.ValidationProblem(resultadoValidacion.ToDictionary());
            //}

            return await next(context);
        }
    }
}
