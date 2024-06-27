using APIERP.Entidades;

namespace APIERP.Repositorios
{
    public interface IRepoErrores
    {
        Task Crear(Error error);
    }
}