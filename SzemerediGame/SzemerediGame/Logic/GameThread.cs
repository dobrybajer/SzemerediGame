using System;
using System.Threading;
using System.Threading.Tasks;
using Object = SzemerediGame.UserInterface.Object;

namespace SzemerediGame.Logic
{
    public class GameThread
    {
        #region Fields for new Thread pausing game

        protected readonly ManualResetEvent Mrse = new ManualResetEvent(true);
        protected Task Task;
        protected CancellationTokenSource Cts;

        #endregion

        protected void Resume()
        {
            Mrse.Set();
        }

        protected void Pause()
        {
            Mrse.Reset();
        }

        protected void CreateTaskPausingGame()
        {
            Cts = new CancellationTokenSource();
            var ct = Cts.Token;
            Task = Task.Factory.StartNew(() =>
            {
                var flag = true;
                while (true)
                {
                    var key = Console.ReadKey(true);
                    if (!ct.IsCancellationRequested)
                    {
                        if (key.Key == ConsoleKey.P)
                        {
                            if (flag) Pause();
                            else Resume();
                            flag = !flag;
                        }
                    }
                    else
                    {
                        ct.ThrowIfCancellationRequested();
                    }
                }
            }, ct);
        }

        protected void DisposeTaskPausingGame()
        {
            Cts.Cancel();
            Console.WriteLine("Wciśnij dowolny klawisz aby zakończyć grę...");
            try
            {
                Task.Wait();
            }
            catch (AggregateException)
            {

            }
            finally
            {
                Cts.Dispose();
                Task.Dispose();
                Object.ClearLines();
            }
        }

    }
}
