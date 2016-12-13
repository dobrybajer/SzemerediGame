namespace SzemerediGame
{
    public interface IGameStrategy
    {
        GameMove Move(Board board);
    }
}