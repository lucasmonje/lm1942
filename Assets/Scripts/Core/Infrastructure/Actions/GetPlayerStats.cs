using Core.Domain.Models;
using Core.Domain.Repositories;

namespace Core.Infrastructure.Actions
{
    public class GetPlayerStats
    {
        private readonly PlayerRepository playerRepository;

        public GetPlayerStats(PlayerRepository playerRepository)
        {
            this.playerRepository = playerRepository;
        }

        public Stats Execute()
        {
            return playerRepository.GetStats();
        }
    }
}