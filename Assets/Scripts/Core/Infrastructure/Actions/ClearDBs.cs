using Core.Domain.Repositories;

namespace Core.Infrastructure.Actions
{
    public class ClearDBs
    {
        private readonly Repository[] repositories;
        private readonly PlayerRepository playerRepository;

        public ClearDBs(params Repository[] repositories)
        {
            this.repositories = repositories;
        }

        public void Execute()
        {
            foreach (var repository in repositories)
            {
                repository.Clear();
            }
        }
    }
}