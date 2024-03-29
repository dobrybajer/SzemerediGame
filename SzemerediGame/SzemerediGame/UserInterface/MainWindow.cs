﻿using System;
using System.Linq;
using SzemerediGame.Helpers;
using SzemerediGame.Logic;
using SzemerediGame.Strategies;

namespace SzemerediGame.UserInterface
{
    internal class MainWindow
    {
        internal void DrawGame()
        {
            var header = new Header(Content.HeaderText);
            var menu = new Menu(Content.MenuOptionsText);
            var gameInput = new GameInput(string.Empty);

            header.WriteContent();
            menu.WriteContent();

            var pressedKey = new ConsoleKeyInfo();
            while (pressedKey.Key != ConsoleKey.Escape)
            {
                pressedKey = Console.ReadKey(true);

                switch (pressedKey.Key)
                {
                    case ConsoleKey.D1:
                        HandleStartGame(menu, gameInput, header);
                        break;
                    case ConsoleKey.D2:
                        Environment.Exit(0);
                        break;
                    default:
                        Console.Clear();
                        header.WriteContent();
                        menu.WriteContent();
                        break;
                }
            }
        }

        private static void HandleStartGame(Object menu, GameInput gameInput, Object header)
        {
            menu.ClearContent();
            gameInput.UpdateText(Content.SubMenuChoiceText);
            gameInput.WriteContent();

            var subPressedKey = new ConsoleKeyInfo();

            while (subPressedKey.Key != ConsoleKey.D1 && subPressedKey.Key != ConsoleKey.D2)
            {
                subPressedKey = Console.ReadKey(true);
            }

            gameInput.ClearContent();

            int[] boardValues = null;
            int? k = null;

            ComputerPlayer player1 = null, player2 = null;

            switch (subPressedKey.Key)
            {
                case ConsoleKey.D1:
                    boardValues = HandleRandomBoardValues(gameInput, out k, out player1, out player2);
                    break;
                case ConsoleKey.D2:
                    boardValues = HandleExplicitBoardValues(gameInput, out k, out player1, out player2);
                    break;
            }

            Console.WriteLine($"Parametry rozgrywki:\nIlość liczb: {boardValues?.Length}\nZwycięska długość ciągu: {k}\nLiczba graczy: 2\n");
            Console.WriteLine(Content.PlayersInformation);

            do
            {
                var game = new GameWithOutput(player1, player2, boardValues, k.Value);
                game.Start();
                
                Console.WriteLine(Content.AfterGameMessage);
                subPressedKey = Console.ReadKey(true);

                game.Reset();
                player1.Reset();
                player2.Reset();

            } while (subPressedKey.Key == ConsoleKey.R);

            Console.Clear();
            header.WriteContent();
            menu.WriteContent();
        }

        private static int[] HandleRandomBoardValues(GameInput gameInput, out int? k, out ComputerPlayer player1, out ComputerPlayer player2)
        {
            int? n = null, a = null;
            k = null;
            player1 = null;
            player2 = null;
            int[] result = null;

            var correctParameters = false;
            while (!correctParameters)
            {
                try
                {
                    n = k = a = null;

                    gameInput.WriteLine(Content.GiveLength);
                    n = int.Parse((string) gameInput.GetContent());

                    gameInput.WriteLine(Content.GiveArithmeticProgressionLength);
                    k = int.Parse((string) gameInput.GetContent());

                    gameInput.WriteLine(Content.GiveLower);
                    a = int.Parse((string) gameInput.GetContent());

                    gameInput.WriteLine(Content.GiveUpper);
                    int? b = int.Parse((string) gameInput.GetContent());

                    GameHelpers.Guard(n > 1, k > 1, n >= k, a < b);

                    result = GameHelpers.GenerateArray(n.Value, a.Value, b.Value);

                    player1 = new ComputerPlayer(ConsoleColor.Red, new MinMaxStrategy(k.Value));
                    player2 = new ComputerPlayer(ConsoleColor.Green, new NaiveStrategy(result, k.Value));

                    correctParameters = true;
                }
                catch (ArgumentException ae)
                {
                    Object.ClearLines(n == null ? 3 : (k == null ? 5 : (a == null ? 7 : 9)));
                    gameInput.WriteLineWrongParameter(ae.ParamName);
                }
                catch
                {
                    Object.ClearLines(n == null ? 3 : (k == null ? 5 : (a == null ? 7 : 9)));
                    gameInput.WriteLineWrongParameter();
                }
            }

            Object.ClearLines(8);

            return result;
        }

        private static int[] HandleExplicitBoardValues(GameInput gameInput, out int? k, out ComputerPlayer player1, out ComputerPlayer player2)
        {
            var correctParameters = false;
            int[] setArray = null;
            k = null;
            player1 = null;
            player2 = null;
            int[] result = null;

            while (!correctParameters)
            {
                try
                {
                    setArray = null;
                    k = null;

                    gameInput.WriteLine(Content.GiveExplicitValues);
                    setArray = ((string)gameInput.GetContent()).Split(new[] { ';' },
                        StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToArray();

                    gameInput.WriteLine(Content.GiveArithmeticProgressionLength);
                    k = int.Parse((string)gameInput.GetContent());

                    GameHelpers.Guard(GameHelpers.Prepare(setArray).Length > 1, k > 1);

                    result = GameHelpers.Prepare(setArray);

                    player1 = new ComputerPlayer(ConsoleColor.Red, new MinMaxStrategy(k.Value));
                    player2 = new ComputerPlayer(ConsoleColor.Green, new NaiveStrategy(result, k.Value));

                    correctParameters = true;
                }
                catch (ArgumentException ae)
                {
                    Object.ClearLines(setArray == null ? 3 : 5);
                    gameInput.WriteLineWrongParameter(ae.ParamName);
                }
                catch
                {
                    Object.ClearLines(setArray == null ? 3 : 5);
                    gameInput.WriteLineWrongParameter();
                }
            }

            Object.ClearLines(5);
            return result;
        }

        private static class Content
        {
            public const string MenuOptionsText = "1. Start;2. Exit";

            public const string HeaderText = "Gra Szemerediego";

            public const string SubMenuChoiceText = "Podaj parametry rozgrywki. Możesz to zrobić na dwa poniższe sposoby.;;" +
                                                    "Naciśnij 1, aby podać jedynie rozmiar zadania (N) oraz zakres losowanych liczb (<a,b>).;" +
                                                    "Naciśnij 2, aby manualnie wprowadzić zbiór liczb naturalnych dla zadania.";

            public const string GiveExplicitValues = "Podaj zbiór liczb naturalnych oddzielonych średnikiem:";

            public const string GiveArithmeticProgressionLength = "Podaj długość ciągu arytmetycznego:";

            public const string GiveLength = "Podaj rozmiar zadania:";

            public const string GiveUpper = "Podaj górną wartość przedziału losowania liczb:";

            public const string GiveLower = "Podaj dolną wartość przedziału losowania liczb:";

            public const string AfterGameMessage = "\nNaciśnij klawisz R aby powtórzyć rozgrywkę dla tych samych danych.\nNaciśnij dowolny inny klawisz, aby powrócić do menu głównego.\n";

            public const string PlayersInformation =
                "Gracz czerwony gra strategią algorytmu min max (heurystyka oceniająca każdy ruch).\n" +
                "Gracz zielony gra strategią naiwną (dokładną), która wylicza wszystkie dostępne ruchy (stąd ograniczenie na rozmiar zadania)\n";
        }
    }
}
