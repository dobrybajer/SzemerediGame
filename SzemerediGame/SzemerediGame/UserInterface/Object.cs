using System;
using System.Drawing;
using System.Linq;
using SzemerediGame.Enums;
using CConsole = Colorful.Console;
using Console = System.Console;

namespace SzemerediGame.UserInterface
{
    internal abstract class Object
    {
        protected string Text;
        protected string[] TextArray;
        protected WritingType Type;
        protected Color Color;
        private readonly char _defaultDelimiter;

        protected Object(string text, WritingType type, Color color, char delimiter)
        {
            Text = text;
            TextArray = text.Split(delimiter).ToArray();
            Type = type;
            Color = color;
            _defaultDelimiter = delimiter;
        }

        public void UpdateText(string text, char? delimiter = null)
        {
            Text = text;
            TextArray = text.Split(delimiter ?? _defaultDelimiter).ToArray();
        }

        public virtual void WriteContent()
        {
            foreach (var text in TextArray)
            {
                WriteLine(text);
            }
        }

        protected void WriteLine(string text, Color? color = null)
        {
            switch (Type)
            {
                case WritingType.Normal:
                    CConsole.WriteLine(text, color ?? Color);
                    break;
                case WritingType.Ascii:
                    CConsole.WriteAscii(text, color ?? Color);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        protected void WriteBackspaceMsg()
        {
            WriteLine(Environment.NewLine + "Wsciśnij BACKSPACE, aby powrocić do menu...");
        }

        public virtual object GetContent()
        {
            var obj = Console.ReadLine();

            return obj;
        }

        public static void ClearLines(int count = 1)
        {
            for (var i = 0; i < count; i++)
            {
                Console.SetCursorPosition(0, Console.CursorTop - 1);
                Console.Write(new string(' ', Console.WindowWidth));
                Console.SetCursorPosition(0, Console.CursorTop - 1);
            }
        }

        public virtual void ClearContent()
        {
            var totalLinesCount = TextArray.Length;
            switch (Type)
            {
                case WritingType.Normal:
                    break;
                case WritingType.Ascii:
                    totalLinesCount *= 6;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            ClearLines(totalLinesCount);
        }
    }
}
