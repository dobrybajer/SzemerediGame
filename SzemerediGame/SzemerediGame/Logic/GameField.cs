namespace SzemerediGame.Logic
{
    public class GameField
    {
        public ComputerPlayer Player { get; set; }
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

        public GameField Clone()
        {
            return new GameField(Value) {Player = Player};
        }
    }
}