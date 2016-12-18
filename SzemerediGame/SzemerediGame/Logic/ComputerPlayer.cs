using System;
using SzemerediGame.Interfaces;

namespace SzemerediGame.Logic
{
    public class ComputerPlayer
    {
        public ConsoleColor Color { get; set; }

        public int Value { get; set; }

        private readonly IGameStrategy _gameStrategy;

        public bool IsAssigned;

        public ComputerPlayer(ConsoleColor color, IGameStrategy gameStrategy)
        {
            Color = color;
            _gameStrategy = gameStrategy;
        }

        public ComputerPlayer()
        {
            Color = ConsoleColor.DarkCyan;
            _gameStrategy = null;
        }

        public GameMove GetMove(Board board)
        {
            return _gameStrategy.Move(board, this);
        }

    }
}