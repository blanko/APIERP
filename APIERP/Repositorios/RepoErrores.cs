using APIERP.Entidades;

namespace APIERP.Repositorios
{
    public class RepoErrores : IRepoErrores
    {
        private readonly ApplicationDbContext context;

        public RepoErrores(ApplicationDbContext context)
        {
            this.context = context;
        }

        public async Task Crear(Error error)
        {
            context.Add(error);
            await context.SaveChangesAsync();
        }
    }
}
