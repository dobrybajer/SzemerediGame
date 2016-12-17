using System;
using System.Linq;
using System.Threading;
using SzemerediGame.Enums;

namespace SzemerediGame.Logic
{
    public class GameWithOutput : Game
    {
        public ConsoleColor DefaultForegroundColor { get; set; }

        public GameWithOutput(ComputerPlayer player1, ComputerPlayer player2, int size, int winningSeriesLength) : base(player1, player2, size, winningSeriesLength)
        {
            DefaultForegroundColor = Console.ForegroundColor;
        }

        public GameWithOutput(ComputerPlayer player1, ComputerPlayer player2, int[] set, int winningSeriesLength) : base(player1, player2, set, winningSeriesLength)
        {
            DefaultForegroundColor = Console.ForegroundColor;
        }
        
        public override GameResult Start()
        {
            Console.WriteLine("Rozpoczęcie gry.");

            return base.Start();
        }

        protected override void GameEnded(GameState result, int[] winningSet)
        {
            if (winningSet != null)
            {
                var winningSortedList = winningSet.ToList();

                for (var i = 0; i < Board.BoardArray.Length; i++)
                {
                    Console.Write(winningSortedList.Contains(i) ? (char) 94 + " " : "  ");
                }
            }
            Console.WriteLine();
            Console.WriteLine("Wynik: " + result);
            Console.WriteLine("Gracz: " + CurrentPlayer.Color);

            base.GameEnded(result, winningSet);
        }

        protected override void PlayerMoved(GameMove move, GameState result)
        {
            foreach (var player in Board.BoardArray)
            {
                if (!player.IsAssigned)
                {
                    Console.Write("_ ");
                    continue;
                }
                Console.ForegroundColor = player.Color;
                Console.Write(player.Value + " ");
                Console.ForegroundColor = DefaultForegroundColor;
            }
            Console.WriteLine();
            Thread.Sleep(1000);
        }
    }
}