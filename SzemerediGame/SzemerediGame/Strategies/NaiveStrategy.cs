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
        private readonly int _winningSetLength;

        private readonly List<int> _excludedFieldsForPlayer = new List<int>();
        private readonly List<int> _excludedFieldsForOponent = new List<int>();

        public NaiveStrategy(IReadOnlyList<int> boardValues, int k)
        {
            _boardValues = boardValues.ToList();
            var n = boardValues[boardValues.Count - 1];

            if (n > MaxNumber) throw new ArgumentOutOfRangeException();

            _winningSetLength = k;

            var allCombinationsCount = Combination_n_of_k(n, k);

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

            var allUnassignedFields = board.BoardArray.Where(gf => !gf.IsAssigned).Select(gf => gf.Value).ToList();

            // Oponent

            var allPlayerFields = board.BoardArray.Where(gf => gf.IsAssigned && gf.Player == player).Select(gf => gf.Value).ToList();
            var availableMovesForOponent = _allMovesSet.Where(m => m.Value == 0 || m.Value == 2).Select(m => m.Key).ToList();

            if (allPlayerFields.Any())
            {
                var newExcludedFieldForOponent = allPlayerFields.Except(_excludedFieldsForOponent).First();
                _excludedFieldsForOponent.Add(newExcludedFieldForOponent);

                var unavailableMovesForOponent = availableMovesForOponent.Where(move => (move & (1 << (newExcludedFieldForOponent - 1))) > 0).ToList();

                foreach (var um in unavailableMovesForOponent)
                {
                    _allMovesSet[um] += 1;
                }
            }

            availableMovesForOponent = _allMovesSet.Where(m => m.Value == 0 || m.Value == 2).Select(m => m.Key).ToList();

            // Player

            var allOpponentFields = board.BoardArray.Where(gf => gf.IsAssigned && gf.Player != player).Select(gf => gf.Value).ToList();
            var availableMovesForPlayer = _allMovesSet.Where(m => m.Value == 0 || m.Value == 1).Select(m => m.Key).ToList();

            if (allOpponentFields.Any())
            {
                var newExcludedFieldForPlayer = allOpponentFields.Except(_excludedFieldsForPlayer).First();
                _excludedFieldsForPlayer.Add(newExcludedFieldForPlayer);

                var unavailableMovesForPlayer = availableMovesForPlayer.Where(move => (move & (1 << (newExcludedFieldForPlayer - 1))) > 0).ToList();

                foreach (var um in unavailableMovesForPlayer)
                {
                    _allMovesSet[um] += 2;
                }
            }

            availableMovesForPlayer = _allMovesSet.Where(m => m.Value == 0 || m.Value == 1).Select(m => m.Key).ToList();

            // Checking whether opponent are going to win very soon

            long closestMoveToWinForOponent = -1;
            var biggestNumberOfCorrectFieldsForOponent = int.MinValue;
            var edgeSituation = false;

            foreach (var move in availableMovesForOponent)
            {
                var correctNumbersCountForOponent = allOpponentFields.Count(field => (move & (1 << (field - 1))) > 0);

                if (correctNumbersCountForOponent > biggestNumberOfCorrectFieldsForOponent)
                {
                    if (CheckIfThisIsEdgeSituation(move, allOpponentFields.Min(), allOpponentFields.Max(), allOpponentFields.Count)) edgeSituation = true;
                    biggestNumberOfCorrectFieldsForOponent = correctNumbersCountForOponent;
                    closestMoveToWinForOponent = move;
                }
                else if (correctNumbersCountForOponent == biggestNumberOfCorrectFieldsForOponent &&
                         CheckIfThisIsEdgeSituation(move, allOpponentFields.Min(), allOpponentFields.Max(), allOpponentFields.Count))
                {
                    biggestNumberOfCorrectFieldsForOponent = correctNumbersCountForOponent;
                    closestMoveToWinForOponent = move;
                    edgeSituation = true;
                }
            }

            // Checking whether player are going to win very soon

            long closestMoveToWinForPlayer = -1;
            var biggestNumberOfCorrectFieldsForPlayer = int.MinValue;
           
            foreach (var move in availableMovesForPlayer)
            {
                var correctNumbersCountForPlayer = allPlayerFields.Count(field => (move & (1 << (field - 1))) > 0);

                if (correctNumbersCountForPlayer > biggestNumberOfCorrectFieldsForPlayer)
                {
                    biggestNumberOfCorrectFieldsForPlayer = correctNumbersCountForPlayer;
                    closestMoveToWinForPlayer = move;
                }
            }

            // Calculations 

            var firstMove = _firstPlayer == null;
            if (!_firstPlayer.HasValue) _firstPlayer = allPlayerFields.Count > allOpponentFields.Count;
            var fieldsForOponentToWin = _winningSetLength - biggestNumberOfCorrectFieldsForOponent;
            var fieldsForPlayerToWin = _winningSetLength - biggestNumberOfCorrectFieldsForPlayer;

            // Coefficients
            
            var itIsADraw = !firstMove && !availableMovesForOponent.Any() && !availableMovesForPlayer.Any();
            var goForWin = !firstMove && !availableMovesForOponent.Any() || fieldsForPlayerToWin == 1;
            var blockOponentNoMoves = !availableMovesForPlayer.Any();
            var blockOponentSoCloseToLose = fieldsForOponentToWin == 1;
            var blockOponentEdgeSituation = fieldsForOponentToWin == 2 && edgeSituation;

            // Selecting first empty field - it is a draw

            if (itIsADraw)
            {
                return new GameMove
                {
                    Index = _boardValues.FindIndex(b => b == board.BoardArray.First(bo => !bo.IsAssigned).Value)
                };
            }


            // Selecting best field - going for win

            if (goForWin)
            {
                var bestMoveFields = CreateIndexArrayFromBits(closestMoveToWinForPlayer);

                return new GameMove
                {
                    Index = bestMoveFields.Except(allPlayerFields).First() - 1// Should be only EXACTLY ONE
                };
            }

            // Selecting best field - blocking oponent due to no available moves for player

            if (blockOponentNoMoves)
            {
                var maxValue = int.MinValue;
                var bestField = -1;

                foreach (var field in allUnassignedFields)
                {
                    var value = availableMovesForOponent.Count(m => (m & (1 << (field - 1))) > 0);

                    if (value > maxValue)
                    {
                        maxValue = value;
                        bestField = field;
                    }
                }

                return new GameMove
                {
                    Index = bestField - 1
                };
            }

            // Selecting best field - blocking oponent cause 1 field to lose

            if (blockOponentSoCloseToLose)
            {
                var bestMoveFields = CreateIndexArrayFromBits(closestMoveToWinForOponent);

                return new GameMove
                {
                    Index = bestMoveFields.Except(allOpponentFields).First() - 1 // Should be only EXACTLY ONE
                };
            }

            // Selecting best field - blocking oponent cause edge situation (possible lose in 2 moves)

            if (blockOponentEdgeSituation)
            {
                var bestMoveFields = CreateIndexArrayFromBits(closestMoveToWinForOponent);
                var availableFieldsForBestMove = bestMoveFields.Except(allOpponentFields).Intersect(allUnassignedFields);
                
                var minValue = double.MaxValue;
                int? closestToTheCenter = null;

                foreach (var am in availableFieldsForBestMove)
                {
                    var value = Math.Abs((double)_boardValues.Count / 2 - am);
                    if (value < minValue)
                    {
                        minValue = value;
                        closestToTheCenter = am;
                    }
                }

                // If somewhing above went wrong (error?) then get first empty field

                return new GameMove
                {
                    Index = closestToTheCenter - 1 ??
                            _boardValues.FindIndex(b => b == board.BoardArray.First(bo => !bo.IsAssigned).Value)
                };
            }

            // Selecting best field - regular situation, looking for field that increases maximally changes to win, and decreases maximally chances to lose for oponent
            {
                var maxValue = int.MinValue;
                var bestField = -1;

                foreach (var field in allUnassignedFields)
                {
                    var x = availableMovesForOponent.Count(m => (m & (1 << (field - 1))) > 0); // Moves blocked for opponent
                    var y = availableMovesForPlayer.Count(m => (m & (1 << (field - 1))) > 0); // Moves allowing player win in future

                    //if (_firstPlayer.Value && x + y > maxValueForField && x <= y ||
                    //    !_firstPlayer.Value && x + y > maxValueForField && x >= y)
                    if (x + y > maxValue)
                    {
                        bestField = field;
                        maxValue = x + y;
                    }
                }

                return new GameMove
                {
                    Index = bestField - 1
                };
            }
            /*
            // Selecting best move
            // TODO Uwzglednic wybieranie ruchu takze na podstawie tego jak bardzo zaszkodzimy przeciwnikowi
            var max = int.MinValue;
            var potentialBestField = 0;
            var bestMove = long.MinValue;
            var ohNoYouAreLosingForBestMove = false;
            var availableMovesForBoth = _allMovesSet.Where(m => m.Value != 3).Select(m => m.Key);
            //TODO na poczatku sprawdzic czy nie jest to sytuacja podbramkowa, a dopiero jesli nie to wybierac najlepzy ruch najpierw szukajac najlepszego pola a potem ruchu zawierajacego to pole
            foreach (var move in availableMovesForBoth)
            {

                // Checking how potential field can influence on my/enemy future possibilities of move
                var list = CreateIndexArrayFromBits(move);
                var bestFieldForMove = -1;
                var maxValueForField = int.MinValue;
                foreach (var a in list)
                {
                    if (!allPlayerFields.Contains(a) && !allOpponentFields.Contains(a))
                    {
                        // TODO jezeli zaczynam to chce max(x+y) tz x < y, w p.p. max(x+y) tz x > y
                        var x = availableMovesForOponent.Count(m => (m & (1 << (a - 1))) > 0); // Moves blocked for opponent
                        var y = availableMovesForPlayer.Count(m => (m & (1 << (a - 1))) > 0); // Moves allowing player win in future

                        //if (_firstPlayer.Value && x + y > maxValueForField && x <= y ||
                        //    !_firstPlayer.Value && x + y > maxValueForField && x >= y)
                        if (x + y > maxValueForField)
                        {
                            bestFieldForMove = a;
                            maxValueForField = x + y;
                        }
                    }
                    
                }

                var correctNumbersCountForPlayer = blockOpponent ? 0 : allPlayerFields.Count(field => (move & (1 << (field - 1))) > 0);

                if (_allMovesSet[move] < 2) correctNumbersCountForPlayer += bias;

                var correctNumbersCountForOponent = goForWin ? 0 : allOpponentFields.Count(field => (move & (1 << (field - 1))) > 0);

                var ohYesYouAreGoingToSmashHim = !firstMove && _winningSetLength - correctNumbersCountForPlayer <= 1;
                //var fieldsForOponentToWin = _winningSetLength - correctNumbersCountForOponent;
                var ohNoYouAreLosing = fieldsForOponentToWin == 1 ||
                                       fieldsForOponentToWin <= 2 &&
                                       CheckIfThisIsEdgeSituation(move, allOpponentFields.Min(), allOpponentFields.Max(), allOpponentFields.Count);

                var endingCondition = ohYesYouAreGoingToSmashHim ? int.MaxValue : (ohNoYouAreLosing ? _winningSetLength : 0);


                if (correctNumbersCountForPlayer + correctNumbersCountForOponent + endingCondition > max)
                {
                    max = correctNumbersCountForPlayer + correctNumbersCountForOponent + endingCondition;
                    potentialBestField = bestFieldForMove;
                    bestMove = move;
                    ohNoYouAreLosingForBestMove = ohNoYouAreLosing;
                }
            }
            
            // Selecting available fields for best move

            if (ohNoYouAreLosingForBestMove)
            {
                var availableFieldsForBestMove = new List<int>();
                for (var p = 0; p < Msb((int)bestMove); p++)
                {
                    if ((bestMove & (1 << p)) > 0 && !allPlayerFields.Contains(p + 1) && !allOpponentFields.Contains(p + 1))
                    {
                        availableFieldsForBestMove.Add(_boardValues.FindIndex(b => b == p + 1));
                    }
                }

                // Selecting field closest to the center from all available fields


                var minValue = double.MaxValue;
                int? closestToTheCenter = null;

                foreach (var am in availableFieldsForBestMove)
                {
                    var value = Math.Abs((double)_boardValues.Count / 2 - _boardValues[am]);
                    if (value < minValue)
                    {
                        minValue = value;
                        closestToTheCenter = am;
                    }
                }

                // If somewhing above went wrong (error?) then get first empty field

                return new GameMove
                {
                    Index =
                        closestToTheCenter ??
                        _boardValues.FindIndex(b => b == board.BoardArray.First(bo => !bo.IsAssigned).Value)
                };
            }
            */
            //return new GameMove { Index = potentialBestField };
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

        private static int Lsb(int v)
        {
            return (int)Math.Log(v - (v & (v - 1)), 2) + 1;
        }

        private static int Msb(int x)
        {
            x |= (x >> 1);
            x |= (x >> 2);
            x |= (x >> 4);
            x |= (x >> 8);
            x |= (x >> 16);
            return (int)Math.Log(x & ~(x >> 1), 2) + 1;
        }

        private static bool CheckIfThisIsEdgeSituation(long move, int minValue, int maxValue, int count)
        {
            var lsbValue = Lsb((int)move);
            var msbValue = Msb((int)move);

            var diff = msbValue - lsbValue - 1;

            return minValue == lsbValue + 1 && maxValue == msbValue - 1 && diff == count;
        }

    }
}
