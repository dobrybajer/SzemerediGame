using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SzemerediGame.Strategies;

namespace SzemerediGame
{
    class Program
    {
        static void Main(string[] args)
        {
            var player1 = new ComputerPlayer(ConsoleColor.Red, new RandomStrategy());
            var player2 = new ComputerPlayer(ConsoleColor.Green, new RandomStrategy());
            var game = new GameWithOutput(player1, player2, 10, 3);

            game.Start();
        }
    }
}
