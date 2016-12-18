using System;
using System.Drawing;
using SzemerediGame.Enums;
using SzemerediGame.Logic;

namespace SzemerediGame.Helpers
{
    public class OutputHelper
    {
        private const char Marker = (char) 94;

        public static string PrintSpaces(int value)
        {
            return new string(' ', value.ToString().Length + 1);
        }

        public static string PrintMarker(int value)
        {

            return Marker + new string(' ', value.ToString().Length);
        }

        public static string TranslateResult(GameState result)
        {
            switch (result)
            {
                case GameState.None:
                    return "Brak";
                case GameState.Tie:
                    return "Remis";
                case GameState.Win:
                    return "Zwycięstwo";
                default:
                    return "Nieznany";
            }
        }

        public static string TranslateColor(ConsoleColor color)
        {
            switch (color)
            {
                case ConsoleColor.Green:
                    return "Zielony";
                case ConsoleColor.Red:
                    return "Czerwony";
                default:
                    return color.ToString();
            }
        }
    }
}