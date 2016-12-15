﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using SzemerediGame.Algorithms;
using SzemerediGame.Enums;

namespace SzemerediGame.Logic
{
    public class Board : ICloneable
    {
        private readonly int _winningSeriesLength;
        public ComputerPlayer[] BoardArray { get; }

        private int _movesCountSoFar;


        public Board(int size, int winningSeriesLength)
        {
            Debug.Assert(size > 1);
            Debug.Assert(winningSeriesLength > 1);

            _winningSeriesLength = winningSeriesLength;

            BoardArray = new ComputerPlayer[size];
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

            BoardArray[move.Index] = player;
            _movesCountSoFar += 1;

            return EvaluateMove(move, player);
        }

        private GameState EvaluateMove(GameMove move, ComputerPlayer player)
        {
            if(IsWinningMove(move, player))
                return GameState.Win;
            
            return IsTieMove() ? GameState.Tie : GameState.None;
        }

        private bool IsTieMove()
        {
            return _movesCountSoFar == BoardArray.Length;
        }

        private bool IsWinningMove(GameMove move, ComputerPlayer player)
        {
            //TODO improvement
            var tab = new List<int>();

            for (var i = 0; i < BoardArray.Length; i++)
            {
                if(BoardArray[i] == player)
                    tab.Add(i);
            }

            return ArithmeticProgression.IsThereAnyProgressionOutThere(tab.ToArray(), _winningSeriesLength);
        }

        public object Clone()
        {
            return new Board(BoardArray, _winningSeriesLength);
        }
    }
}