using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SzemerediGame.Algorithms;
using SzemerediGame.Interfaces;
using SzemerediGame.Logic;

namespace SzemerediGame.Strategies
{
    class ImprovedRandomStrategy : IGameStrategy
    {
        private readonly int _winningSequenceLength;

        private readonly Random _random;

        public ImprovedRandomStrategy()
        {
            _winningSequenceLength = 0;

            _random = new Random();
        }

        public ImprovedRandomStrategy(int k)
        {
            _winningSequenceLength = k;

            _random = new Random();
        }

        public GameMove Move(Board board, ComputerPlayer player)
        {
            var allPlayerFields = board.BoardArray.Where(gf => gf.IsAssigned && gf.Player == player).Select(gf => gf.Value);
            var allOpponentFields = board.BoardArray.Where(gf => gf.IsAssigned && gf.Player != player).Select(gf => gf.Value);

            var potentialMoves = new List<int>();

            for (int i = 0; i < board.BoardArray.Length; i++)
            {
                if (board.BoardArray[i].IsAssigned) continue;

                var playerFieldsList = allPlayerFields.ToList();
                playerFieldsList.Add(board.BoardArray[i].Value);
                playerFieldsList.Sort();

                var opponentFieldsList = allOpponentFields.ToList();
                opponentFieldsList.Add(board.BoardArray[i].Value);
                opponentFieldsList.Sort();

                if (ArithmeticProgression.IsThereAnyProgressionOutThere(playerFieldsList.ToArray(), _winningSequenceLength) ||
                    ArithmeticProgression.IsThereAnyProgressionOutThere(opponentFieldsList.ToArray(), _winningSequenceLength))
                {
                    return new GameMove()
                    {
                        Index = i
                    };
                }

                potentialMoves.Add(i);
            }

            var randomIndex = _random.Next(0, potentialMoves.Count - 1);

            return new GameMove { Index = potentialMoves[randomIndex] };
        }

        public void Reset()
        {
            
        }
    }
}
