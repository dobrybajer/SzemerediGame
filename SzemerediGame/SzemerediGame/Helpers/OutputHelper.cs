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
    }
}