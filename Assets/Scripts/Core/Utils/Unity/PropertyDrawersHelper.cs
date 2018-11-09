using System.Linq;
using Core.Infrastructure.Factories;

namespace Core.Utils.Unity
{
    public static class PropertyDrawersHelper
    {
        public static string[] GetAllEnemies()
        {
            return GameProvider.ProvideConfiguration().AllEnemies.Select(e => e.Name).ToArray();
        }
        
        public static string[] GetAllGuns()
        {
            return GameProvider.ProvideConfiguration().AllGuns.Select(g => g.Name).ToArray();
        }
        
        public static string[] GetAllPaths()
        {
            return GameProvider.ProvideConfiguration().AllPaths.Select(p => p.Name).ToArray();
        }
    }
}