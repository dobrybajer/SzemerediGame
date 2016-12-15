using System;
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


        public override GameResult Start()
        {
            Console.WriteLine("Rozpoczęcie gry.");

            return base.Start();
        }

        protected override void GameEnded(GameState result)
        {
            Console.WriteLine("Wynik: " + result.ToString());
            Console.WriteLine("Gracz: " + CurrentPlayer.Color.ToString());
        }

        protected override void PlayerMoved(GameMove move, GameState result)
        {
            foreach (var player in Board.BoardArray)
            {
                if (player == null)
                {
                    Console.Write("_ ");
                    continue;
                }
                Console.ForegroundColor = player.Color;
                Console.Write("X ");
                Console.ForegroundColor = DefaultForegroundColor;
            }
            Console.WriteLine();
            Thread.Sleep(1000);
        }
    }
}