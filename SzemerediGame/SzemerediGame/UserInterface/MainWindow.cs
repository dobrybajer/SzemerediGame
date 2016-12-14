using System;
using SzemerediGame.Logic;
using SzemerediGame.Strategies;

namespace SzemerediGame.UserInterface
{
    internal class MainWindow
    {
        internal void DrawGame()
        {
            var player1 = new ComputerPlayer(ConsoleColor.Red, new RandomStrategy());
            var player2 = new ComputerPlayer(ConsoleColor.Green, new RandomStrategy());
            var game = new GameWithOutput(player1, player2, 10, 3);
            var header = new Header("Gra Szemerediego");
            var menu = new Menu("1. Start;2. About;3. Exit");
            const string descriptionContent = "Na początku rozgrywki komputer losuje zbiór liczb;" +
                                              "naturalnych X. Każdy gracz ma swój własny kolor.;" +
                                              "Ruch polega na wybraniu niepokolorowanej liczby ze;" +
                                              "zbioru X i pokolorowaniu jej swoim kolorem.;" +
                                              "Gracze ścigają się kto pierwszy ułoży monochromatyczny;" +
                                              "ciąg arytmetyczny o zadanej długości k.;" +
                                              "Zadanie: Symulacja gry komputer kontra komputer, testy.";
            var description = new Description(descriptionContent);

            header.WriteContent();
            menu.WriteContent();

            var pressedKey = new ConsoleKeyInfo();
            while (pressedKey.Key != ConsoleKey.Escape)
            {


                pressedKey = Console.ReadKey(true);

                switch (pressedKey.Key)
                {
                    case ConsoleKey.D1:
                        game.Start();
                        break;
                    case ConsoleKey.D2:
                        description.WriteContent();
                        break;
                    case ConsoleKey.D3:
                        Environment.Exit(0);
                        break;
                    case ConsoleKey.Backspace:
                        Console.Clear();
                        header.WriteContent();
                        menu.WriteContent();
                        break;
                    default:
                        Console.Clear();
                        header.WriteContent();
                        menu.WriteContent();
                        break;
                }
            }
        }
    }
}
