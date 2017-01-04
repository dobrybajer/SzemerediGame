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

        private readonly Dictionary<long, ushort> _allMovesSet;
        private readonly List<int> _boardValues;

        private bool? _firstPlayer;
        private int _winningSetLength;

        private readonly List<int> _excludedFieldsForPlayer = new List<int>();
        private readonly List<int> _excludedFieldsForOponent = new List<int>();

        public NaiveStrategy(IReadOnlyList<int> boardValues, int k)
        {
            _boardValues = boardValues.ToList();
            var n = boardValues[boardValues.Count - 1];

            if (n > MaxNumber) throw new ArgumentOutOfRangeException();

            _winningSetLength = k;

            var allCombinationsCount = Combination_n_of_k(n, k);
            //_allMovesSet = new List<long>();
            _allMovesSet = new Dictionary<long, ushort>();

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
                if(flag && ArithmeticProgression.IsThereAnyProgressionOutThere(CreateIndexArrayFromBits(a), k)) _allMovesSet.Add(a, 0);

                a = next_set_of_n_elements(a);
            }
        }

        public GameMove Move(Board board, ComputerPlayer player)
        {
            // TODO check situation when this strategy starts game

            // Oponent

            var allPlayerFields = board.BoardArray.Where(gf => gf.IsAssigned && gf.Player == player).Select(gf => gf.Value).ToList();
            var availableMovesForOponent = new List<long>();

            if (allPlayerFields.Any())
            {
                var newExcludedFieldForOponent = allPlayerFields.Except(_excludedFieldsForOponent).First();
                _excludedFieldsForOponent.Add(newExcludedFieldForOponent);

                availableMovesForOponent = _allMovesSet.Where(m => m.Value == 0 || m.Value == 2).Select(m => m.Key).ToList();
                var unavailableMovesForOponent = availableMovesForOponent.Where(move => (move & (1 << (newExcludedFieldForOponent - 1))) > 0).ToList();

                foreach (var um in unavailableMovesForOponent)
                {
                    _allMovesSet[um] += 1;
                }
            }

            // Player

            var allOpponentFields = board.BoardArray.Where(gf => gf.IsAssigned && gf.Player != player).Select(gf => gf.Value).ToList();
            var availableMovesForPlayer = new List<long>();

            if (allOpponentFields.Any())
            {
                var newExcludedFieldForPlayer = allOpponentFields.Except(_excludedFieldsForPlayer).First();
                _excludedFieldsForPlayer.Add(newExcludedFieldForPlayer);

                availableMovesForPlayer = _allMovesSet.Where(m => m.Value == 0 || m.Value == 1).Select(m => m.Key).ToList();
                var unavailableMovesForPlayer = availableMovesForPlayer.Where(move => (move & (1 << (newExcludedFieldForPlayer - 1))) > 0).ToList();

                foreach (var um in unavailableMovesForPlayer)
                {
                    _allMovesSet[um] += 2;
                }
            }

            // Coefficients

            var firstMove = _firstPlayer == null;
            if (!_firstPlayer.HasValue) _firstPlayer = allPlayerFields.Count > allOpponentFields.Count;
            var bias = firstMove && !_firstPlayer.Value ? 2 : 0;
            var goForWin = !firstMove && !availableMovesForOponent.Any(); // TODO Uwzglednic jak blisko jestesmy wygranej
            var blockOpponent = !availableMovesForPlayer.Any(); // TODO Uwzglednic jak blisko jest wygranej przeciwnik


            // Selecting best move
            // TODO Uwzglednic wybieranie ruchu takze na podstawie tego jak bardzo zaszkodzimy przeciwnikowi
            // TODO Uwzglednic sytuacje kiedy przeciwnik ma x_____y i może na 100% wygrać
            var max = int.MinValue;
            var bestMove = long.MinValue;

            var availableMovesForBoth = _allMovesSet.Where(m => m.Value != 3).Select(m => m.Key);
            foreach (var move in availableMovesForBoth)
            {
                var correctNumbersCountForPlayer = blockOpponent ? 0 : allPlayerFields.Count(field => (move & (1 << (field - 1))) > 0);

                if (_allMovesSet[move] < 2) correctNumbersCountForPlayer += bias;

                var correctNumbersCountForOponent = goForWin ? 0 : allOpponentFields.Count(field => (move & (1 << (field - 1))) > 0);

                if (correctNumbersCountForPlayer + correctNumbersCountForOponent > max)
                {
                    max = correctNumbersCountForPlayer + correctNumbersCountForOponent;
                    bestMove = move;
                }
            }

            // Selecting first empty field from best move

            for (var p = 0; p < MaxNumber; p++)
            {
                if ((bestMove & (1 << p)) > 0 && !allPlayerFields.Contains(p+1) && !allOpponentFields.Contains(p + 1))
                {
                    return new GameMove { Index = _boardValues.FindIndex(b => b == p + 1) };
                }
            }

            // If somewhing above went wrong (error?) then get first empty field

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
