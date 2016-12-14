using SzemerediGame.Enums;

namespace SzemerediGame.Logic
{
    public class Game
    {
        private readonly ComputerPlayer _player1;
        private readonly ComputerPlayer _player2;
        protected ComputerPlayer CurrentPlayer;
        protected readonly Board Board;

        public Game(ComputerPlayer player1, ComputerPlayer player2, int size, int winningSeriesLength)
        {
            _player1 = player1;
            _player2 = player2;

            CurrentPlayer = _player2; // Celowo ustawiony drugi gracz
            Board = new Board(size, winningSeriesLength);
        }


        public virtual GameResult Start()
        {
            GameState result;
            do
            {
                SwitchPlayers();
                var move = CurrentPlayer.GetMove(Board);
                result = Board.MakeMove(move, CurrentPlayer);
                PlayerMoved(move, result);
            } while (result == GameState.None);

            GameEnded(result);

            return new GameResult(result,CurrentPlayer);
        }

        protected virtual void PlayerMoved(GameMove move, GameState result)
        {
            //Nop
        }

        protected virtual void GameEnded(GameState result)
        {
            //Nop
        }

        private void SwitchPlayers()
        {
            CurrentPlayer = CurrentPlayer == _player1 ? _player2 : _player1;
        }
    }
}

