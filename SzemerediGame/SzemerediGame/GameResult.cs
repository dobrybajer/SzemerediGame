namespace SzemerediGame
{
    public class GameResult
    {
        public GameState GameState { get; }
        public ComputerPlayer Winner { get; }

        public GameResult(GameState gameState, ComputerPlayer currentPlayer)
        {
            GameState = gameState;
            Winner = gameState == GameState.Win ? currentPlayer : null;
        }
    }
}