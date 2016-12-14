using System.Drawing;
using SzemerediGame.Enums;

namespace SzemerediGame.UserInterface
{
    internal class Header : Object
    {
        public Header(string text, WritingType type = WritingType.Ascii, Color color = new Color(), char delimiter = ';') : base(text, type, color, delimiter)
        {
            Color = Color.FromArgb(123, 34, 189);
        }
    }
}
