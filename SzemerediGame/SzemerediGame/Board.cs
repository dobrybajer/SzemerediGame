using System;
using System.Diagnostics;

namespace SzemerediGame
{
    public class Board : ICloneable
    {
        private readonly int _winningSeriesLength;
        public ComputerPlayer[] BoardArray { get; }

        private int _movesCountSoFar = 0;


        public Board(int size, int winningSeriesLength)
        {
            Debug.Assert(size > 1);
            Debug.Assert(winningSeriesLength > 1);

            _winningSeriesLength = winningSeriesLength;

            BoardArray = new ComputerPlayer[size];
        }

        private Board(ComputerPlayer[] board, int winningSeriesLength)
        {
            BoardArray = new ComputerPlayer[BoardArray.Length];

            for (var i = 0; i < board.Length; i++)
                BoardArray[i] = board[i];

            _winningSeriesLength = winningSeriesLength;
        }

        public GameState MakeMove(GameMove move, ComputerPlayer player)
        {
            Debug.Assert(move.Index >= 0);
            Debug.Assert(move.Index < BoardArray.Length);

            BoardArray[move.Index] = player;
            _movesCountSoFar += 1;

            return EvaluateMove(move);
        }

        private GameState EvaluateMove(GameMove move)
        {
            if(IsWinningMove(move))
                return GameState.Win;
            
            return IsTieMove() ? GameState.Tie : GameState.None;
        }

        private bool IsTieMove()
        {
            return _movesCountSoFar == BoardArray.Length;
        }

        private bool IsWinningMove(GameMove move)
        {
            //TODO implementacja sprawdzenia wygranej

            return false;
        }

        public object Clone()
        {
            return new Board(BoardArray, _winningSeriesLength);
        }
    }
}