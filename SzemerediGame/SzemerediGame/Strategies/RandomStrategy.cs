using System;
using System.Collections.Generic;
using SzemerediGame.Interfaces;
using SzemerediGame.Logic;

namespace SzemerediGame.Strategies
{
    public class RandomStrategy : IGameStrategy
    {
        private readonly Random _random;

        public RandomStrategy()
        {
            _random = new Random();
        }

        public GameMove Move(Board board, ComputerPlayer player)
        {
            //return new GameMove { Index = int.Parse(Console.ReadLine()) };

            var potentialMoves = new List<int>();
            for (var i = 0; i < board.BoardArray.Length; i++)
            {
                if (!board.BoardArray[i].IsAssigned)
                {
                    potentialMoves.Add(i);
                }
            }

            var randomIndex = _random.Next(0, potentialMoves.Count - 1);

            return new GameMove { Index = potentialMoves[randomIndex] };
        }
    }
}