using System;
using System.Collections.Generic;
using System.Linq;

namespace SzemerediGame.Helpers
{
    public class GameHelpers
    {
        public static int[] GenerateArray(int n, int a, int b)
        {
            return Prepare(Enumerable.Range(a, b - a).OrderBy(g => Guid.NewGuid()).Take(n));
        }

        public static int[] Prepare(IEnumerable<int> collection)
        {
            return collection.OrderBy(x => x).Distinct().ToArray();
        }

        public static void Guard(params bool[] conditions)
        {
            if (conditions.Any(condition => !condition))
            {
                throw new ArgumentException();
            }
        }
    }
}