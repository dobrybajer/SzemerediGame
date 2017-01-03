using System;
using System.Collections.Generic;
using System.Linq;
using SzemerediGame.Algorithms;
using SzemerediGame.Interfaces;
using SzemerediGame.Logic;

namespace SzemerediGame.Strategies
{
    internal class NaiveStrategy : IGameStrategy
    {
        private const int MaxNumber = 29;

        private List<long> _allMovesSet;
        private readonly List<int> _boardValues;

        private readonly List<int> _excludedFields = new List<int>();

        public NaiveStrategy(IReadOnlyList<int> boardValues, int k)
        {
            _boardValues = boardValues.ToList();
            var n = boardValues[boardValues.Count - 1];

            if (n > MaxNumber) throw new ArgumentOutOfRangeException();

            var allCombinationsCount = Combination_n_of_k(n, k);
            _allMovesSet = new List<long>();

            var a = (long) Math.Pow(2, k) - 1; 

            for (long i = 0; i < allCombinationsCount; i++)
            {
                var flag = true;
                var p = 0;
                while (1 << p <= a)
                {
                    if ((a & (1 << p)) > 0 && !boardValues.Contains(p+1))
                    {
                        flag = false;
                        break;
                    }
                    p++;
                }
                if(flag && ArithmeticProgression.IsThereAnyProgressionOutThere(CreateIndexArrayFromBits(a), k)) _allMovesSet.Add(a);

                a = next_set_of_n_elements(a);
            }
        }

        public GameMove Move(Board board, ComputerPlayer player)
        {
            var allPlayerFields = board.BoardArray.Where(gf => gf.IsAssigned && gf.Player == player).Select(gf => gf.Value).ToList();
            var allOpponentFields = board.BoardArray.Where(gf => gf.IsAssigned && gf.Player != player).Select(gf => gf.Value).ToList();

            var newExcludedField = allOpponentFields.Except(_excludedFields).First();
            _excludedFields.Add(newExcludedField);

            var unavailableMoves = _allMovesSet.Where(move => (move & (1 << (newExcludedField - 1))) > 0).ToList();
            _allMovesSet = _allMovesSet.Except(unavailableMoves).ToList();
            
            // TODO jesli nie ma juz dostepnych ruchow - odpal strategie blokujaca przeciwnika
            
            var max = int.MinValue;
            var bestMove = long.MinValue;

            foreach (var move in _allMovesSet)
            {
                var correctNumbersCount = allPlayerFields.Count(field => (move & (1 << (field - 1))) > 0);

                if (correctNumbersCount > max)
                {
                    max = correctNumbersCount;
                    bestMove = move;
                }
            }

            // TODO do best move dodaj sprawdzanie blokowania przeciwnika: wsp_1 * correctNumbersCount + wsp_2 * (1 - blockedNumbersCount)
            // TODO uwzglednic czy startujemy jako pierwsi czy jako drudzy

            for (var p = 0; p < MaxNumber; p++)
            {
                if ((bestMove & (1 << p)) > 0 && !allPlayerFields.Contains(p+1))
                {
                    return new GameMove { Index = _boardValues.FindIndex(b => b == p + 1) };
                }
            }

            return new GameMove { Index = _boardValues.FindIndex(b => b == board.BoardArray.First(bo => !bo.IsAssigned).Value) };
        }
        
        private static long next_set_of_n_elements(long x)
        {
            if (x == 0) return 0;

            var smallest = x & -x;
            var ripple = x + smallest;
            var newSmallest = ripple & -ripple;
            var ones = ((newSmallest / smallest) >> 1) - 1;
            return ripple | ones;
        }

        private static long Combination_n_of_k(long n, long k)
        {
            if (k > n) return 0;
            if (k == 0 || k == n) return 1;

            if (k * 2 > n) k = n - k;

            long r = 1;
            for (long d = 1; d <= k; ++d)
            {
                r *= n--;
                r /= d;
            }

            return r;
        }

        private static long BitCount(long n)
        {
            if (n <= 0) return 0;

            var uCount = n - ((n >> 1) & 033333333333) - ((n >> 2) & 011111111111);
            return ((uCount + (uCount >> 3)) & 030707070707) % 63;
        }

        private static int[] CreateIndexArrayFromBits(long value)
        {
            var list = new List<int>();

            var p = 0;
            while (1 << p < value)
            {
                if ((value & (1 << p)) > 0)
                    list.Add(p + 1);
                p++;
            }

            return list.ToArray();
        }
    }
}
