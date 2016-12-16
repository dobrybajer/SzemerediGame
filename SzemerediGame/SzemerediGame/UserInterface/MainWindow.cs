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
                        int? n = null, k = null, a = null, b = null;
                        var correctParameters = false;

                        if (subPressedKey.Key == ConsoleKey.D1)
                        {
                            while (!correctParameters)
                            {
                                try
                                {
                                    setArray = null;
                                    gameInput.WriteLine("Podaj zbiór liczb naturalnych oddzielonych średnikiem:");
                                    var input1 = gameInput.GetContent() as string;
                                    setArray = input1.Split(';').Select(int.Parse).ToArray();

                                    gameInput.WriteLine("Podaj długość ciągu arytmetycznego:");
                                    var input2 = gameInput.GetContent() as string;
                                    k = int.Parse(input2);

                                    correctParameters = true;
                                }
                                catch
                                {
                                    Object.ClearLines(setArray == null ? 3 : 5);
                                    gameInput.WriteLineWrongParameter();
                                }
                            }

                            Object.ClearLines(5);

                            game.Start(/*setArray, k.Value*/);
                        }
                        else if (subPressedKey.Key == ConsoleKey.D2)
                        {
                            while (!correctParameters)
                            {
                                try
                                {
                                    n = null;
                                    k = null;
                                    a = null;
                                    b = null;

                                    gameInput.WriteLine("Podaj rozmiar zadania:");
                                    var input1 = gameInput.GetContent() as string;
                                    n = int.Parse(input1);

                                    gameInput.WriteLine("Podaj długość ciągu arytmetycznego:");
                                    var input2 = gameInput.GetContent() as string;
                                    k = int.Parse(input2);

                                    gameInput.WriteLine("Podaj dolną wartość przedziału losowania liczb:");
                                    var input3 = gameInput.GetContent() as string;
                                    a = int.Parse(input3);

                                    gameInput.WriteLine("Podaj górną wartość przedziału losowania liczb:");
                                    var input4 = gameInput.GetContent() as string;
                                    b = int.Parse(input4);

                                    correctParameters = true;
                                }
                                catch
                                {
                                    Object.ClearLines(n == null ? 3 : (k == null ? 5 : (a == null ? 7 : 9)));
                                    gameInput.WriteLineWrongParameter();
                                }
                            }

                            Object.ClearLines(8);

                            game.Start(/*n.Value, k.Value, a.Value, b.Value*/);
                        }

                        Console.WriteLine("Naciśnij dowolny klawisz, aby powrócić do menu głównego...");
                        Console.ReadKey(true);
                        Console.Clear();
                        header.WriteContent();
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
