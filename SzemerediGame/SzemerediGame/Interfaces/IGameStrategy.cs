using SzemerediGame.Logic;

namespace SzemerediGame.Interfaces
{
    public interface IGameStrategy
    {
        GameMove Move(Board board, ComputerPlayer player);
    }
}