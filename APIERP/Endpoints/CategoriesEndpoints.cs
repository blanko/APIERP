using APIERP.DTOs;
using APIERP.Entidades;
using APIERP.Repositorios;
using APIERP.Servicios;
using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;

namespace APIERP.Endpoints
{
    public static class CategoriesEndpoints
    {
        private static readonly string container = "categories";
        public static RouteGroupBuilder MapCategories(this RouteGroupBuilder group)
        {
            group.MapGet("/", GetAll).CacheOutput(c => c.Expire(TimeSpan.FromSeconds(60)).Tag("categories-get"));
            group.MapGet("/{id:int}", GetById);
            //group.MapPost("/", Add).AddEndpointFilter<FiltroValidaciones<CategoryDTOAdd>>();
            group.MapPost("/", Add).DisableAntiforgery();
            //group.MapPut("/{id:int}", Update).AddEndpointFilter<FiltroValidaciones<CategoryDTOAdd>>();
            group.MapPut("/{id:int}", Update).DisableAntiforgery();
            group.MapDelete("/{id:int}", Delete);
            return group;
        }

        static async Task<Ok<List<CategoryDTO>>> GetAll(IRepoCategories repo, IMapper mapper)
        {
            var categorias = await repo.GetAll();
            var categoriasDTO = mapper.Map<List<CategoryDTO>>(categorias);
            return TypedResults.Ok(categoriasDTO);
        }

        static async Task<Results<Ok<CategoryDTO>, NotFound>> GetById(int id, IRepoCategories repo,
            IMapper mapper)
        {
            var categoria = await repo.GetById(id);

            if (categoria is null)
            {
                return TypedResults.NotFound();
            }

            var CategoryDTO = mapper.Map<CategoryDTO>(categoria);

            return TypedResults.Ok(CategoryDTO);
        }

        static async Task<Ok<List<CategoryDTO>>> GetByName(string nombre,
           IRepoCategories repo, IMapper mapper)
        {
            var categories = await repo.GetByName(nombre);
            var categoriesDTO = mapper.Map<List<CategoryDTO>>(categories);
            return TypedResults.Ok(categoriesDTO);
        }

        static async Task<Results<Created<CategoryDTO>, ValidationProblem>> Add(
            [FromForm] CategoryDTOAdd CategoryDTOAdd, IRepoCategories repo,
            IOutputCacheStore outputCacheStore, IMapper mapper,
            IAlmacenadorArchivos almacenadorArchivos)
        {
            var categoria = mapper.Map<Category>(CategoryDTOAdd);

            if (CategoryDTOAdd.Image is not null)
            {
                var url = await almacenadorArchivos.Almacenar(container, CategoryDTOAdd.Image);
                categoria.ImageUrl = url;
            }

            var id = await repo.Add(categoria);
            await outputCacheStore.EvictByTagAsync("categories-get", default);
            var CategoryDTO = mapper.Map<CategoryDTO>(categoria);
            return TypedResults.Created($"/categories/{id}", CategoryDTO);
        }

        static async Task<Results<NoContent, NotFound, ValidationProblem>> Update(int id,
            [FromForm] CategoryDTOAdd CategoryDTOAdd, IRepoCategories repo,
            IOutputCacheStore outputCacheStore, IMapper mapper,
            IAlmacenadorArchivos almacenadorArchivos)
        {
            var categoriaDB = await repo.GetById(id);

            if (categoriaDB is null)
            {
                return TypedResults.NotFound();
            }

            var categoria = mapper.Map<Category>(CategoryDTOAdd);
            categoria.CategoryId = id;
            categoria.ImageUrl = categoriaDB.ImageUrl;

            if (CategoryDTOAdd.Image is not null)
            {
                var url = await almacenadorArchivos.Editar(categoria.ImageUrl,
                    container, CategoryDTOAdd.Image);
                categoria.ImageUrl = url;
            }


            await repo.Update(categoria);
            await outputCacheStore.EvictByTagAsync("categories-get", default);
            return TypedResults.NoContent();
        }

        static async Task<Results<NoContent, NotFound>> Delete(int id, IRepoCategories repo,
            IOutputCacheStore outputCacheStore,
            IAlmacenadorArchivos almacenadorArchivos)
        {
            var categoriaDB = await repo.GetById(id);

            if (categoriaDB is null)
            {
                return TypedResults.NotFound();
            }

            await repo.Delete(id);
            await almacenadorArchivos.Borrar(categoriaDB.ImageUrl, container);
            await outputCacheStore.EvictByTagAsync("categories-get", default);
            return TypedResults.NoContent();
        }
    }
}
