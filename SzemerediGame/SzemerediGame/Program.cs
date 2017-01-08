using System;
using Colorful;
using SzemerediGame.Algorithms;
using SzemerediGame.Logic;
using SzemerediGame.Strategies;
using SzemerediGame.UserInterface;
using Console = System.Console;

namespace SzemerediGame
{
    internal class Program
    {
        private static void Main()
        {
            var mainWindow = new MainWindow();
            mainWindow.DrawGame();
        }
    }
}
