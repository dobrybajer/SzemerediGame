﻿using System;
using System.Collections.Generic;

namespace SzemerediGame.Algorithms
{
    internal static class ArithmeticProgression
    {
        public static bool IsThereAnyProgressionOutThere(int[] set, int k)
        {
            var n = set.Length;
            var gapSize = n/(double) (k-1);
            var dMax = Math.Ceiling(gapSize)* (set[set.Length - 1] - set[0])/n;

            var p = n/(double)(k + 1);

            for (var i0 = p; i0 < n; i0 += p)
            {
                var i0R = (int) i0;

                for (var i = i0R; i < n; ++i)
                {
                    if (set[i] <= set[i0R] + dMax)
                    {
                        for (var j = i + 1; j < n; ++j)
                        {
                            if (set[j] <= set[i] + dMax)
                            {
                                var diff = set[j] - set[i];
                                var sequenceCount = 2;
                                var z = i;
                                for (var r = i-1; r >= 0; r--)
                                {
                                    if (set[z] - set[r] == diff)
                                    {
                                        z = r;
                                        sequenceCount++;
                                        if (sequenceCount >= k) return true;
                                    }
                                }
                                z = j;
                                for (var l = j + 1; l < n; l++)
                                {
                                    if (set[l] - set[z] == diff)
                                    {
                                        z = l;
                                        sequenceCount++;
                                        if (sequenceCount >= k) return true;
                                    }
                                }

                                if (sequenceCount >= k) return true;
                            }
                            else break;
                        }
                    }
                    else break;
                }
                
            }

            return false;
        }

        public static int[] IsThereAnyProgressionOutThere_WithWinnigSet(int[] set, int k)
        {
            var n = set.Length;
            var gapSize = n / (double)(k - 1);
            var dMax = Math.Ceiling(gapSize) * (set[set.Length - 1] - set[0]) / n;

            var p = n / (double)(k + 1);

            for (var i0 = p; i0 < n; i0 += p)
            {
                var i0R = (int)i0;

                for (var i = i0R; i < n; ++i)
                {
                    if (set[i] <= set[i0R] + dMax)
                    {
                        for (var j = i + 1; j < n; ++j)
                        {
                            if (set[j] <= set[i] + dMax)
                            {
                                var diff = set[j] - set[i];
                                var sequenceCount = 2;
                                var winningSet = new List<int> {set[i], set[j]};
                                var z = i;
                                for (var r = i - 1; r >= 0; r--)
                                {
                                    if (set[z] - set[r] == diff)
                                    {
                                        z = r;
                                        sequenceCount++;
                                        winningSet.Add(set[r]);
                                        if (sequenceCount >= k) return winningSet.ToArray();
                                    }
                                }
                                z = j;
                                for (var l = j + 1; l < n; l++)
                                {
                                    if (set[l] - set[z] == diff)
                                    {
                                        z = l;
                                        sequenceCount++;
                                        winningSet.Add(set[l]);
                                        if (sequenceCount >= k) return winningSet.ToArray();
                                    }
                                }

                                if (sequenceCount >= k) return winningSet.ToArray();
                            }
                            else break;
                        }
                    }
                    else break;
                }

            }

            return null;
        }
    }
}
