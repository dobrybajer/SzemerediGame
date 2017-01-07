using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using SzemerediGame.Algorithms;
using SzemerediGame.Interfaces;
using SzemerediGame.Logic;

namespace SzemerediGame.Strategies
{
    public class MinMaxStrategy : IGameStrategy
    {
        private readonly int _winningSequenceLength;

        private const int Depth = 2;

        private ComputerPlayer _maxPlayer;
        private ComputerPlayer _minPlayer;

        public MinMaxStrategy()
        {
            _winningSequenceLength = 0;
        }

        public MinMaxStrategy(int k)
        {
            _winningSequenceLength = k;
        }

        public GameMove Move(Board board, ComputerPlayer player)
        {
            _maxPlayer = player;
            _minPlayer = FindOpponentPlayer(board, player);
            var move = -1;

            AlphaBeta((Board)board.Clone(), Depth, int.MinValue, int.MaxValue, ref move);
            return new GameMove() { Index = move };
        }

        private ComputerPlayer FindOpponentPlayer(Board board, ComputerPlayer player)
        {
            var opponent = board.BoardArray.Where(x => x.IsAssigned).Select(x => x.Player).FirstOrDefault(p => p != player);
            return opponent ?? new ComputerPlayer();
        }

        private void CountPotentialOneMoveWinsAndLoses(Board board, out int wins, out int loses)
        {
            wins = 0;
            loses = 0;

            var allPlayerFields = board.BoardArray.Where(gf => gf.IsAssigned && gf.Player == _maxPlayer).Select(gf => gf.Value);
            var allOpponentFields = board.BoardArray.Where(gf => gf.IsAssigned && gf.Player != _maxPlayer).Select(gf => gf.Value);

            foreach (GameField gameField in board.BoardArray)
            {
                if (gameField.IsAssigned) continue;

                var playerFieldsList = allPlayerFields.ToList();
                playerFieldsList.Add(gameField.Value);
                playerFieldsList.Sort();

                var opponentFieldsList = allOpponentFields.ToList();
                opponentFieldsList.Add(gameField.Value);
                opponentFieldsList.Sort();

                if (ArithmeticProgression.IsThereAnyProgressionOutThere(playerFieldsList.ToArray(), _winningSequenceLength)) wins++;

                if (ArithmeticProgression.IsThereAnyProgressionOutThere(opponentFieldsList.ToArray(), _winningSequenceLength)) loses++;
            }
        }

        private int Heuristic(Board board, int depth)
        {
            var isOpponent = IsOpponentMove(depth);

            var value = 0;

            if (board.WinningSet != null && board.WinningSet.Length > 0)
            {
                // NOTE If this is my move now and there already is a winning sequence, it means that the it's the opponent
                // who have the winning sequence. Otherwise, if the opponent is moving now and there already is a winning
                // sequence, it means that I have created it.
                value = isOpponent ? -100 : 100;
            }
            else
            {
                int oneMoveWins;
                int oneMoveLoses;

                CountPotentialOneMoveWinsAndLoses(board, out oneMoveWins, out oneMoveLoses);

                value += oneMoveWins * 50;
                value -= oneMoveLoses * 50;
            }

            // TODO tu reszta do zaimplementowania
            // TODO atakowanie, blokowanie

            return value;
        }

        private int AlphaBeta(Board board, int depth, int alpha, int beta, ref int move)
        {
            var isOpponent = IsOpponentMove(depth);

            if (depth == 0 || board.BoardArray.All(x => x.IsAssigned) || (board.WinningSet != null && board.WinningSet.Length > 0))
            {
                return Heuristic(board, depth + 1);
            }

            return isOpponent ? MakeMinMove(board, depth, alpha, beta, ref move) : MakeMaxMove(board, depth, alpha, beta, ref move);
        }

        private int MakeMaxMove(Board board, int depth, int alpha, int beta, ref int move)
        {
            foreach (var index in GetAllAvaliableMoves(board))
            {
                var gameMove = new GameMove() { Index = index };
                board.MakeMove(gameMove, _maxPlayer);

                var value = AlphaBeta(board, depth - 1, alpha, beta, ref move);

                if (value > alpha)
                {
                    alpha = value;
                    move = index;
                }

                board.ClearMove(gameMove);
                if (alpha >= beta)
                    break;
            }

            return alpha;
        }

        private int MakeMinMove(Board board, int depth, int alpha, int beta, ref int move)
        {
            foreach (var index in GetAllAvaliableMoves(board))
            {
                var gameMove = new GameMove() { Index = index };
                board.MakeMove(gameMove, _minPlayer);

                var value = AlphaBeta(board, depth - 1, alpha, beta, ref move);

                if (value < beta)
                {
                    beta = value;
                    move = index;
                }

                board.ClearMove(gameMove);
                if (alpha >= beta)
                    break;
            }

            return beta;
        }

        private static bool IsOpponentMove(int depth)
        {
            return depth % 2 != Depth % 2;
        }

        private IEnumerable<int> GetAllAvaliableMoves(Board board)
        {
            for (var i = 0; i < board.BoardArray.Length; i++)
            {
                if (!board.BoardArray[i].IsAssigned)
                {
                    yield return i;
                }
            }
        }
    }
}
