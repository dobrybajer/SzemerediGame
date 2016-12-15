using System;
using System.Linq;
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
            var gameInput = new GameInput("");
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
                        menu.ClearContent();
                        gameInput.UpdateText("Wciśnij 1, aby podać zbiór liczb naturalnych dla zadania;Wciśnij 2, aby podać rozmiar zadania (N) oraz zakres losowanych liczb (<a,b>)");
                        gameInput.WriteContent();
        
                        var subPressedKey = Console.ReadKey(true);
                        while (subPressedKey.Key != ConsoleKey.D1 && subPressedKey.Key != ConsoleKey.D2) { }

                        gameInput.ClearContent();

                        int[] setArray = null;
                        int[] argsArray = null;
                        var correctParameters = false;

                        if (subPressedKey.Key == ConsoleKey.D1)
                        {
                            while (!correctParameters)
                            {
                                gameInput.UpdateText("Podaj zbiór liczb naturalnych oddzielonych średnikiem");

                                try
                                {
                                    gameInput.WriteContent();
                                    var input = gameInput.GetContent() as string;
                                    setArray = input.Split(';').Select(int.Parse).ToArray();
                                    correctParameters = true;
                                }
                                catch
                                {
                                    gameInput.ClearLines(3);
                                    gameInput.WriteLineWrongParameter();
                                }
                            }

                            gameInput.ClearLines(3);

                            game.Start(/*setArray*/);
                        }
                        else if (subPressedKey.Key == ConsoleKey.D2)
                        {
                            while (!correctParameters)
                            {
                                gameInput.UpdateText("Podaj rozmiar zadania, a następnie zakres losowanych liczb <a,b> (wartości oddzielone średnkiem)");

                                try
                                {
                                    gameInput.WriteContent();
                                    var input = gameInput.GetContent() as string;
                                    argsArray = input.Split(';').Select(int.Parse).ToArray();
                                    if (argsArray.Length != 3) throw new ArgumentException();
                                    correctParameters = true;
                                }
                                catch
                                {
                                    gameInput.ClearLines(3);
                                    gameInput.WriteLineWrongParameter();
                                }
                            }

                            gameInput.ClearLines(3);

                            game.Start(/*argsArray[0], argsArray[1]*/);
                        }

                        Console.WriteLine("Naciśnij dowolny klawisz, aby powrócić do menu głównego...");
                        Console.ReadKey();
                        menu.ClearLines(Console.BufferHeight - 6);
                        menu.WriteContent();

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
