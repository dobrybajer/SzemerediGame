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
        public GameField[] BoardArray { get; }
        public int[] WinningSet { get; private set; }

        private int _movesCountSoFar;

        public Board(IReadOnlyList<int> set, int winningSeriesLength)
        {
            Debug.Assert(set.Count > 1);
            Debug.Assert(winningSeriesLength > 1);

            _winningSeriesLength = winningSeriesLength;

            BoardArray = new GameField[set.Count];

            for (var i = 0; i < BoardArray.Length; i++)
            {
                BoardArray[i] = new GameField(set[i]);
            }
        }

        private Board(IReadOnlyList<GameField> board, int winningSeriesLength)
        {
            BoardArray = new GameField[board.Count];

            for (var i = 0; i < board.Count; i++)
                BoardArray[i] = board[i];

            _winningSeriesLength = winningSeriesLength;
        }

        public GameState MakeMove(GameMove move, ComputerPlayer player)
        {
            Debug.Assert(move.Index >= 0);
            Debug.Assert(move.Index < BoardArray.Length);

            BoardArray[move.Index].AssignToField(player);
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
            //TODO użyć 'move' do opytmalizacji
            var tab = new List<int>();

            foreach (GameField gameField in BoardArray)
            {
                if (gameField.IsAssigned && gameField.Player == player)
                    tab.Add(gameField.Value);
            }

            return ArithmeticProgression.IsThereAnyProgressionOutThere_WithWinnigSet(tab.ToArray(), _winningSeriesLength);
        }

        public object Clone()
        {
            return new Board(BoardArray, _winningSeriesLength);
        }

        public void ClearMove(GameMove move)
        {
            _movesCountSoFar -= 1;
            WinningSet = null;
            BoardArray[move.Index].AssignToField(null);
        }
    }
}