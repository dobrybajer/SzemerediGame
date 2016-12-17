namespace SzemerediGame.Logic
{
    public class GameField
    {
        public ComputerPlayer Player { get; private set; }
        public int Value { get; set; }
        public bool IsAssigned => Player != null;

        public GameField(int value)
        {
            Value = value;
        }

        public void AssignToField(ComputerPlayer player)
        {
            Player = player;
        }


    }
}