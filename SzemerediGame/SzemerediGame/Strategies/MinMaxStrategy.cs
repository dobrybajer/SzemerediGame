using System.Collections.Generic;
using System.Linq;
using SzemerediGame.Algorithms;
using SzemerediGame.Interfaces;
using SzemerediGame.Logic;

namespace SzemerediGame.Strategies
{
    public class MinMaxStrategy : IGameStrategy
    {
        private readonly int _winningSequenceLength;
        private readonly int _depth;

        private ComputerPlayer _maxPlayer;
        private ComputerPlayer _minPlayer;

        public MinMaxStrategy(int k, int depth = 10)
        {
            _winningSequenceLength = k;
            _depth = depth;
        }

        public GameMove Move(Board board, ComputerPlayer player)
        {
            _maxPlayer = player;
            _minPlayer = FindOpponentPlayer(board, player);

            var move = AlphaBeta((Board)board.Clone(), _depth, int.MinValue, int.MaxValue);
            return new GameMove() { Index = move };
        }

        public void Reset()
        {
            
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

            var allPlayerFields = board.BoardArray.Where(gf => gf.IsAssigned && gf.Player == _maxPlayer).Select(gf => gf.Value).ToList();
            var allOpponentFields = board.BoardArray.Where(gf => gf.IsAssigned && gf.Player != _maxPlayer).Select(gf => gf.Value).ToList();

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
                value = isOpponent ? -10000 - (_depth - depth) * 100 : 10000 + depth * 100;

                // Count how many other options are there for winning / losing (except the current winning set).
                int totalOtherWins = 0;
                int totalOtherLoses = 0;

                for (int i = 0; i < board.BoardArray.Length; i++)
                {
                    if (!board.WinningSet.Contains(board.BoardArray[i].Value)) continue;

                    Board tempBoard = (Board)board.Clone();

                    int otherWins;
                    int otherLoses;

                    tempBoard.BoardArray[i].AssignToField(null);

                    CountPotentialOneMoveWinsAndLoses(tempBoard, out otherWins, out otherLoses);

                    totalOtherWins += otherWins > 0 ? otherWins - 1 : otherWins;
                    totalOtherLoses += otherLoses > 0 ? otherLoses - 1 : otherLoses;
                }

                value += totalOtherWins * 60;
                value -= totalOtherLoses * 60;
            }
            else
            {
                int oneMoveWins;
                int oneMoveLoses;

                CountPotentialOneMoveWinsAndLoses(board, out oneMoveWins, out oneMoveLoses);

                value += oneMoveWins * 50;
                value -= oneMoveLoses * 50;
            }

            // TODO ewentualne usprawnienia do heurystyki
            // preferencja centralnych pól itp

            return value;
        }

        private int AlphaBeta(Board board, int depth, int alpha, int beta)
        {
            var isOpponent = IsOpponentMove(depth);

            if (depth == 0 || board.BoardArray.All(x => x.IsAssigned) || (board.WinningSet != null && board.WinningSet.Length > 0))
            {
                // NOTE If this is my move now and there already is a winning sequence, it means that the it's the opponent
                // who have the winning sequence. Otherwise, if the opponent is moving now and there already is a winning
                // sequence, it means that I have created it.
                return Heuristic(board, depth + 1);
            }

            return isOpponent ? MakeMinMove(board, depth, alpha, beta) : MakeMaxMove(board, depth, alpha, beta);
        }

        private int MakeMaxMove(Board board, int depth, int alpha, int beta)
        {
            var move = -1;
            foreach (var index in GetAllAvaliableMoves(board))
            {
                var gameMove = new GameMove() { Index = index };
                board.MakeMove(gameMove, _maxPlayer);

                var value = AlphaBeta(board, depth - 1, alpha, beta);

                if (value > alpha)
                {
                    move = index;
                    alpha = value;
                }

                board.ClearMove(gameMove);
                if (alpha >= beta)
                    break;
            }

            if (depth == _depth)
                return move;

            return alpha;
        }

        private int MakeMinMove(Board board, int depth, int alpha, int beta)
        {
            foreach (var index in GetAllAvaliableMoves(board))
            {
                var gameMove = new GameMove() { Index = index };
                board.MakeMove(gameMove, _minPlayer);

                var value = AlphaBeta(board, depth - 1, alpha, beta);

                if (value < beta)
                {
                    beta = value;
                }

                board.ClearMove(gameMove);
                if (alpha >= beta)
                    break;
            }

            return beta;
        }

        private bool IsOpponentMove(int depth)
        {
            return depth % 2 != _depth % 2;
        }

        private static IEnumerable<int> GetAllAvaliableMoves(Board board)
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
