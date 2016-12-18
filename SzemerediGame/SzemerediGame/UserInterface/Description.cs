using System.Drawing;
using SzemerediGame.Enums;

namespace SzemerediGame.UserInterface
{
    internal class Description : Object
    {
        public Description(string text, WritingType type = WritingType.Normal, Color color = new Color(), char delimiter = ';') : base(text, type, color, delimiter)
        {
        }

        public override void WriteContent()
        {
            var r = 225;
            const int g = 255;
            var b = 250;

            foreach (var t in TextArray)
            {
                WriteLine(t, Color.FromArgb(r, g, b));
                r -= 18;
                b -= 9;
            }

            WriteBackspaceMsg();
        }

        public override void ClearContent()
        {
            ClearLines(6);
        }
    }
}
