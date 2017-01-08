using System.Drawing;
using SzemerediGame.Enums;

namespace SzemerediGame.UserInterface
{
    internal class GameInput : Object
    {
        public GameInput(string text, WritingType type = WritingType.Normal, Color color = new Color(), char delimiter = ';') : base(text, type, color, delimiter)
        {
            Color = Color.AliceBlue;
        }

        public void WriteLineWrongParameter()
        {
            WriteLine("Niepoprawnie podane dane wejściowe, proszę spróbować ponownie.", Color.DarkRed);
        }

        public void WriteLineWrongParameter(string argument)
        {
            WriteLine("Zbyt duża wartość danych wejściowych dla strategii gracza \"NaiveStrategy\": " + argument, Color.DarkRed);
        }
    }
}
