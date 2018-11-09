using System;
using System.Collections.Generic;
using System.Linq;
using Core.Domain.Models;

namespace Core.Domain.Utils
{
    public static class WeightedRandomization
    {
        private static readonly Random Rand = new Random();

        public static T Choose<T>(List<T> list) where T : Weighted
        {
            if (list.Count == 0) return default(T);

            var totalweight = list.Sum(c => c.Weight);
            var choice = Rand.Next(totalweight);
            var sum = 0;

            foreach (var obj in list)
            {
                for (var i = sum; i < obj.Weight + sum; i++)
                    if (i >= choice)
                        return obj;

                sum += obj.Weight;
            }

            return list.First();
        }
    }
}