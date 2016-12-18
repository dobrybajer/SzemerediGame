using System;
using System.Linq;
using System.Threading;
using SzemerediGame.Enums;
using SzemerediGame.Helpers;
using SzemerediGame.UserInterface;

namespace SzemerediGame.Logic
{
    public class GameWithOutput : Game
    {
        public ConsoleColor DefaultForegroundColor { get; set; }

        public GameWithOutput(ComputerPlayer player1, ComputerPlayer player2, int[] set, int winningSeriesLength) : base(player1, player2, set, winningSeriesLength)
        {
            DefaultForegroundColor = Console.ForegroundColor;
        }

        public override GameResult Start()
        {
            Console.WriteLine("Rozpoczęcie gry.");
            Console.WriteLine();

            return base.Start();
        }

        protected override void GameEnded(GameState result, int[] winningSet)
        {
            DrawWinningSet(winningSet);
            DrawGameResult(result);

            DisposeTaskPausingGame();
        }

        private void DrawWinningSet(int[] winningSet)
        {
            if (winningSet != null)
            {
                var winningSortedList = winningSet.ToList();

                for (var i = 0; i < Board.BoardArray.Length; i++)
                {
                    Console.Write(winningSortedList.Contains(i)
                        ? OutputHelper.PrintMarker(Board.BoardArray[i].Value)
                        : OutputHelper.PrintSpaces(Board.BoardArray[i].Value));
                }
            }
        }

        private void DrawGameResult(GameState result)
        {
            Console.WriteLine();
            Console.WriteLine("Wynik: " + result);

            if (result != GameState.Win) return;

            Console.ForegroundColor = CurrentPlayer.Color;
            Console.WriteLine("Gracz: " + CurrentPlayer.Color);
            Console.ForegroundColor = DefaultForegroundColor;
        }

        protected override void PlayerMoved(GameMove move, GameState result)
        {
            foreach (var player in Board.BoardArray)
            {
                if (!player.IsAssigned)
                {
                    Console.Write(player.Value + " ");
                    continue;
                }
                Console.ForegroundColor = player.Player.Color;
                Console.Write(player.Value + " ");
                Console.ForegroundColor = DefaultForegroundColor;
            }
            Console.WriteLine();
            Thread.Sleep(1000);
        }
    }
}