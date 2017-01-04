using System;
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
            var gameInput = new GameInput(String.Empty);
           // var description = new Description(Content.DescriptionContentText);

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
                    //case ConsoleKey.D2:
                    //    description.WriteContent();
                    //    description.ClearContent();
                    //    break;
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

        private static void HandleStartGame(Menu menu, GameInput gameInput, Header header)
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

            switch (subPressedKey.Key)
            {
                case ConsoleKey.D1:
                    boardValues = HandleRandomBoardValues(gameInput, out k);
                    break;
                case ConsoleKey.D2:
                    boardValues = HandleExplicitBoardValues(gameInput, out k);
                    break;
            }

            Console.WriteLine($"Parametry rozgrywki:\nIlość liczb: {boardValues?.Length}\nZwycięska długość ciągu: {k}\nLiczba graczy: 2\n");
            Console.WriteLine("Gracz czerwony gra strategią zupełnie losową (wybiera dowolne z niezajętych jeszcze pól).\n" +
                              "Gracz zielony gra usprawnioną strategią losową tzn.:\n" +
                              "\tJeśli w danym ruchu jest pole które daje mu natychmiastową wygraną/przegraną to maluje te pole,\n" +
                              "\taby odpowiednio wygrać/zablokować). W pozostałych przypadkach rusza się losowo.\n");

            do
            {
                var player1 = new ComputerPlayer(ConsoleColor.Red, new RandomStrategy());
                //var player2 = new ComputerPlayer(ConsoleColor.Green, new ImprovedRandomStrategy(k.Value));
                var player2 = new ComputerPlayer(ConsoleColor.Green, new NaiveStrategy(boardValues, k.Value));

                var game = new GameWithOutput(player1, player2, boardValues, k.Value);
                game.Start();

                Console.WriteLine("\nNaciśnij klawisz R aby powtórzyć rozgrywkę dla tych samych danych.\nNaciśnij dowolny inny klawisz, aby powrócić do menu głównego.\n");
                subPressedKey = Console.ReadKey(true);
            } while (subPressedKey.Key == ConsoleKey.R);

            Console.Clear();
            header.WriteContent();
            menu.WriteContent();
        }

        private static int[] HandleRandomBoardValues(GameInput gameInput, out int? k)
        {
            int? n = null, a = null, b = null;
            k = null;

            var correctParameters = false;
            while (!correctParameters)
            {
                try
                {
                    n = k = a = b = null;

                    gameInput.WriteLine(Content.GiveLength);
                    n = int.Parse((string)gameInput.GetContent());

                    gameInput.WriteLine(Content.GiveArithmeticProgressionLength);
                    k = int.Parse((string)gameInput.GetContent());

                    gameInput.WriteLine(Content.GiveLower);
                    a = int.Parse((string)gameInput.GetContent());

                    gameInput.WriteLine(Content.GiveUpper);
                    b = int.Parse((string)gameInput.GetContent());

                    GameHelpers.Guard(n > 1, k > 1, n >= k, a < b);

                    correctParameters = true;
                }
                catch
                {
                    Object.ClearLines(n == null ? 3 : (k == null ? 5 : (a == null ? 7 : 9)));
                    gameInput.WriteLineWrongParameter();
                }
            }

            Object.ClearLines(8);

            return GameHelpers.GenerateArray(n.Value, a.Value, b.Value);
        }

        private static int[] HandleExplicitBoardValues(GameInput gameInput, out int? k)
        {
            var correctParameters = false;
            int[] setArray = null;
            k = null;

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


                    correctParameters = true;
                }
                catch
                {
                    Object.ClearLines(setArray == null ? 3 : 5);
                    gameInput.WriteLineWrongParameter();
                }
            }

            Object.ClearLines(5);
            return GameHelpers.Prepare(setArray);
        }

        private static class Content
        {
            public const string DescriptionContentText = "Na początku rozgrywki komputer losuje zbiór liczb;" +
                                             "naturalnych X. Każdy gracz ma swój własny kolor.;" +
                                             "Ruch polega na wybraniu niepokolorowanej liczby ze;" +
                                             "zbioru X i pokolorowaniu jej swoim kolorem.;" +
                                             "Gracze ścigają się kto pierwszy ułoży monochromatyczny;" +
                                             "ciąg arytmetyczny o zadanej długości k.;" +
                                             "Zadanie: Symulacja gry komputer kontra komputer, testy.";

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
        }
    }
}
