using System.Collections.Generic;
using Core.Domain.Utils;

namespace Core.Domain.Models
{
    public class PlayerPoints
    {
        private readonly List<Point> points;

        public PlayerPoints(List<Point> points)
        {
            this.points = points;
        }

        public Point Choose()
        {
            return WeightedRandomization.Choose(points);
        }

        public List<Point> GetPoints()
        {
            return points;
        }
    }
}