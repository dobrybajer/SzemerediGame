using System.Drawing;
using SzemerediGame.Enums;

namespace SzemerediGame.UserInterface
{
    internal class Menu : Object
    {
        public Menu(string text, WritingType type = WritingType.Normal, Color color = new Color(), char delimiter = ';') : base(text, type, color, delimiter)
        {
            Color = Color.Aqua;
        }
    }
}
