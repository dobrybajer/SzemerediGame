using System.Collections.Generic;
using System.Linq;
using SzemerediGame.Interfaces;
using SzemerediGame.Logic;

namespace SzemerediGame.Strategies
{
    public class MinMaxStrategy : IGameStrategy
    {

        private const int Depth = 6;

        private ComputerPlayer _maxPlayer;
        private ComputerPlayer _minPlayer;

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

        private static int Heuristic(Board board, int depth)
        {
            var value = 0;

            if (board.WinningSet != null && board.WinningSet.Length > 0)
            {
                value = IsOpponentMove(depth) ? -100 : 100;
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
