using System;
using System.Collections.Generic;
using System.Drawing;
using CConsole = Colorful.Console;
using Console = System.Console;

namespace SzemerediGame
{
    internal class Program
    {
        private static bool _isMenuRendered;

        private static void Main()
        {


            var pressedKey = new ConsoleKeyInfo();
            while (pressedKey.Key != ConsoleKey.Escape)
            {
                if (!_isMenuRendered)
                {
                    GetHeader();
                    GetMenu();

                    _isMenuRendered = !_isMenuRendered;
                }

                pressedKey = Console.ReadKey(true);

                switch (pressedKey.Key)
                {
                    case ConsoleKey.D1:
                        break;
                    case ConsoleKey.D2:
                        break;
                    case ConsoleKey.D3:
                        ClearLine(4);
                        GetGameDescription();
                        break;
                    case ConsoleKey.D4:
                        Environment.Exit(0);
                        break;
                    case ConsoleKey.Backspace:
                        Console.Clear();
                        GetHeader();
                        GetMenu();
                        break;
                    default:
                        GetUnknownCommandMessage();
                        break;
                }
            }
            
        }


        private static void GetHeader()
        {
            var title = "Gra Szemerediego";
            CConsole.WriteAscii(title, Color.FromArgb(123, 34, 189));
        }

        private static void GetGameDescription()
        {
            var descriptionLines = new List<string>
            {
                "Na początku rozgrywki komputer losuje zbiór liczb ",
                "naturalnych X. Każdy gracz ma swój własny kolor.",
                "Ruch polega na wybraniu niepokolorowanej liczby ze ",
                "zbioru X i pokolorowaniu jej swoim kolorem.",
                "Gracze ścigają się kto pierwszy ułoży monochromatyczny ",
                "ciąg arytmetyczny o zadanej długości k.",
                "Zadanie: Symulacja gry komputer kontra komputer, testy."
            };


            var r = 225;
            var g = 255;
            var b = 250;
            foreach (var t in descriptionLines)
            {
                CConsole.WriteLine(t, Color.FromArgb(r, g, b));

                r -= 18;
                b -= 9;
            }

            CConsole.WriteLine(Environment.NewLine + "Wsciśnij BACKSPACE, aby powrocić do menu...");
        }

        private static void GetMenu()
        {
            CConsole.WriteLine("1. Start");
            CConsole.WriteLine("2. Controls");
            CConsole.WriteLine("3. About");
            CConsole.WriteLine("4. Exit");
        }

        private static void GetUnknownCommandMessage()
        {
            CConsole.WriteLine("Niepoprawny wybór, proszę wybrać cyfrę z przedziału <1, 4>.");
        }


        public static void ClearLine(int lines = 1)
        {
            for (var i = 1; i <= lines; i++)
            {
                Console.SetCursorPosition(0, Console.CursorTop - 1);
                Console.Write(new string(' ', Console.WindowWidth));
                Console.SetCursorPosition(0, Console.CursorTop - 1);
            }
        }
    }
}
