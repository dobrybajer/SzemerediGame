using System;
using System.Collections.Generic;
using System.Diagnostics;
using SzemerediGame.Algorithms;
using SzemerediGame.Enums;

namespace SzemerediGame.Logic
{
    public class Board : ICloneable
    {
        private readonly int _winningSeriesLength;
        public ComputerPlayer[] BoardArray { get; }
        public int[] WinningSet { get; private set; }

        private int _movesCountSoFar;


        public Board(int size, int winningSeriesLength)
        {
            Debug.Assert(size > 1);
            Debug.Assert(winningSeriesLength > 1);

            _winningSeriesLength = winningSeriesLength;

            BoardArray = new ComputerPlayer[size];

            for (var i = 0; i < BoardArray.Length; i++)
            {
                BoardArray[i] = new ComputerPlayer { Value = i + 1 };
            }
        }

        public Board(IReadOnlyList<int> set, int winningSeriesLength)
        {
            Debug.Assert(set.Count > 1);
            Debug.Assert(winningSeriesLength > 1);

            _winningSeriesLength = winningSeriesLength;

            BoardArray = new ComputerPlayer[set.Count];

            for (var i = 0; i < BoardArray.Length; i++)
            {
                BoardArray[i] = new ComputerPlayer {Value = set[i]};
            }
        }

        private Board(IReadOnlyList<ComputerPlayer> board, int winningSeriesLength)
        {
            BoardArray = new ComputerPlayer[BoardArray.Length];

            for (var i = 0; i < board.Count; i++)
                BoardArray[i] = board[i];

            _winningSeriesLength = winningSeriesLength;
        }

        public GameState MakeMove(GameMove move, ComputerPlayer player)
        {
            Debug.Assert(move.Index >= 0);
            Debug.Assert(move.Index < BoardArray.Length);

            BoardArray[move.Index].Color = player.Color;
            BoardArray[move.Index].IsAssigned = true;
            _movesCountSoFar += 1;

            return EvaluateMove(move, player);
        }

        private GameState EvaluateMove(GameMove move, ComputerPlayer player)
        {
            var winningSet = IsWinningMove(move, player);
            if (winningSet != null)
            {
                WinningSet = winningSet;
                return GameState.Win;
            }
            
            return IsTieMove() ? GameState.Tie : GameState.None;
        }

        private bool IsTieMove()
        {
            return _movesCountSoFar == BoardArray.Length;
        }

        private int[] IsWinningMove(GameMove move, ComputerPlayer player)
        {
            //TODO improvement
            var tab = new List<int>();

            for (var i = 0; i < BoardArray.Length; i++)
            {
                if(BoardArray[i].IsAssigned && BoardArray[i].Color == player.Color)
                    tab.Add(i);
            }

            return ArithmeticProgression.IsThereAnyProgressionOutThere_WithWinnigSet(tab.ToArray(), _winningSeriesLength);
        }

        public object Clone()
        {
            return new Board(BoardArray, _winningSeriesLength);
        }
    }
}