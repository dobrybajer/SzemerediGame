using System;
using System.Drawing;

namespace SzemerediGame
{
    public class ComputerPlayer
    {
        public ConsoleColor Color { get; }
        private readonly IGameStrategy _gameStrategy;

        public ComputerPlayer(ConsoleColor color, IGameStrategy gameStrategy)
        {
            Color = color;
            _gameStrategy = gameStrategy;
        }


        public GameMove GetMove(Board board)
        {
            return _gameStrategy.Move(board);
        }

    }
}