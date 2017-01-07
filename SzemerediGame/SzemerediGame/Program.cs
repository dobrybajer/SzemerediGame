using System;
using Colorful;
using SzemerediGame.Algorithms;
using SzemerediGame.Logic;
using SzemerediGame.Strategies;
using SzemerediGame.UserInterface;

namespace SzemerediGame
{
    internal class Program
    {
        private static void Main()
        {
            int[] set = {1, 3, 4, 7, 9, 10, 12, 13};
            int[] set2 = { 1, 3, 5, 7, 9 };

            int[] set3 =
            {
                694000000, 694000002, 694000006, 694000007, 694000009, 694000010,
                694000013, 694000015, 694000018, 694000019, 694000021, 694000022, 694000023,
                694000026, 694000028, 694000030, 694000034, 694000036, 694000038, 694000040,
                694000043, 694000045, 694000046, 694000048, 694000051, 694000053, 694000055,
                694000057, 694000060, 694000061, 694000063, 694000067, 694000069, 694000072,
                694000074, 694000076, 694000077, 694000079, 694000080, 694000082, 694000083,
                694000084, 694000086, 694000090, 694000091, 694000093, 694000095, 694000099,
                694000102, 694000103, 694000105, 694000108, 694000109, 694000113, 694000116,
                694000118, 694000122, 694000125, 694000128, 694000131, 694000134, 694000137,
                694000141, 694000143, 694000145, 694000148, 694000152, 694000153, 694000154,
                694000157, 694000160, 694000162, 694000163, 694000166, 694000170, 694000173,
                694000174, 694000177, 694000179, 694000180, 694000181, 694000184, 694000185,
                694000187, 694000189, 694000193, 694000194, 694000198, 694000200, 694000203,
                694000207, 694000211, 694000215, 694000219, 694000222, 694000226, 694000228,
                694000232, 694000235, 694000236
            };

            //var ap = new ArithmeticProgression();
            //var r1 = ap.IsThereAnyProgressionOutThere(set, 5);
            //var r2 = ap.IsThereAnyProgressionOutThere(set2, 5);
            //var r3 = ap.IsThereAnyProgressionOutThere(set3, 10);

            //Console.WriteLine(r1);
            //Console.WriteLine(r2);
            //Console.WriteLine(r3);

            //var mainWindow = new MainWindow();
            //mainWindow.DrawGame();


            var player1 = new ComputerPlayer(ConsoleColor.Red, new NaiveStrategy(new[] { 1, 2, 3, 4, 5, 6 }, 3));
            //var player2 = new ComputerPlayer(ConsoleColor.Green, new ImprovedRandomStrategy(k.Value));
            var player2 = new ComputerPlayer(ConsoleColor.Green, new MinMaxStrategy());

            var game = new GameWithOutput(player1, player2, new[] {1,2,3,4,5,6}, 3);
            game.Start();
        }
    }
}
