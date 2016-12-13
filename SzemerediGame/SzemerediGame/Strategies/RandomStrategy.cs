using System;
using System.Collections.Generic;

namespace SzemerediGame.Strategies
{
    public class RandomStrategy : IGameStrategy
    {
        private readonly Random _random;

        public RandomStrategy()
        {
            _random = new Random();
        }

        public GameMove Move(Board board)
        {
            var potentialMoves = new List<int>();
            for (var i = 0; i < board.BoardArray.Length; i++)
            {
                if (board.BoardArray[i] == null)
                {
                    potentialMoves.Add(i);
                }
            }

            var randomIndex = _random.Next(0, potentialMoves.Count - 1);

            return new GameMove() { Index = potentialMoves[randomIndex] };
        }
    }
}