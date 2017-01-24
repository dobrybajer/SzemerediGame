using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SzemerediGame.Enums;
using SzemerediGame.Helpers;
using SzemerediGame.Logic;
using SzemerediGame.Strategies;

namespace StrategiesTests
{
    [TestClass]
    // Testy z restowaniem, gramy na tej samej planszy 100 razy
    public class StrategiesTests
    {
        public const int GamesIterations = 100;

        public const int K = 4;

        public const int N = 13;

        /// <summary>
        /// Losowany początek przedziału
        /// </summary>
        public const int A = 1;

        /// <summary>
        /// Losowany koniec przedziału
        /// </summary>
        public const int B = 14; // x < B

        public const int MinMaxDepth = 6;

        private void BattlesImplementation(ComputerPlayer player1, ComputerPlayer player2, List<int> boardValues)
        {
            var game = new Game(player1, player2, boardValues, K);

            var resultList = new List<GameResult>();

            //Act
            for (var i = 0; i < GamesIterations; i++)
            {
                var result = game.Start();
                resultList.Add(result);
                game.Reset();
                player1.Reset();
                player2.Reset();
            }

            //Assert
            Console.WriteLine("Ties: " + resultList.Count(x => x.GameState == GameState.Tie));
            Console.WriteLine(player1.StrategyName + ": " + resultList.Count(x => x.GameState == GameState.Win && x.Winner == player1));
            Console.WriteLine(player2.StrategyName + ": " + resultList.Count(x => x.GameState == GameState.Win && x.Winner == player2));
        }

        /// <summary>
        /// _ vs RandomStrategy
        /// </summary>

        [TestMethod]
        public void ImprovedRandomStrategy_vs_RandomStrategy()
        {
            
            var boardValues = GameHelpers.GenerateArray(N, A, B).ToList();
            var player1 = new ComputerPlayer(ConsoleColor.Green, new ImprovedRandomStrategy(K));
            var player2 = new ComputerPlayer(ConsoleColor.Red, new RandomStrategy());

            BattlesImplementation(player1, player2, boardValues);

            Assert.IsTrue(true);
        }


        [TestMethod]
        public void NaiveStrategy_vs_RandomStrategy()
        {
            
            var boardValues = GameHelpers.GenerateArray(N, A, B).ToList();
            var player1 = new ComputerPlayer(ConsoleColor.Green, new NaiveStrategy(boardValues, K));
            var player2 = new ComputerPlayer(ConsoleColor.Red, new RandomStrategy());

            BattlesImplementation(player1, player2, boardValues);

            Assert.IsTrue(true);
        }


        [TestMethod]
        public void MinMaxStrategy_vs_RandomStrategy()
        {
            
            var boardValues = GameHelpers.GenerateArray(N, A, B).ToList();
            var player1 = new ComputerPlayer(ConsoleColor.Green, new MinMaxStrategy(K, MinMaxDepth));
            var player2 = new ComputerPlayer(ConsoleColor.Red, new RandomStrategy());

            BattlesImplementation(player1, player2, boardValues);

            Assert.IsTrue(true);
        }

        /// <summary>
        /// _ vs ImprovedRandomStrategy
        /// </summary>

        [TestMethod]
        public void RandomStrategy_vs_ImprovedRandomStrategy()
        {
            
            var boardValues = GameHelpers.GenerateArray(N, A, B).ToList();
            var player1 = new ComputerPlayer(ConsoleColor.Green, new RandomStrategy());
            var player2 = new ComputerPlayer(ConsoleColor.Red, new ImprovedRandomStrategy(K));

            BattlesImplementation(player1, player2, boardValues);

            Assert.IsTrue(true);
        }

        [TestMethod]
        public void NaiveStrategy_vs_ImprovedRandomStrategy()
        {
            var boardValues = GameHelpers.GenerateArray(N, A, B).ToList();
            var player1 = new ComputerPlayer(ConsoleColor.Green, new NaiveStrategy(boardValues, K));
            var player2 = new ComputerPlayer(ConsoleColor.Red, new ImprovedRandomStrategy(K));

            BattlesImplementation(player1, player2, boardValues);

            Assert.IsTrue(true);
        }

        [TestMethod]
        public void MinMaxStrategy_vs_ImprovedRandomStrategy()
        {            
            var boardValues = GameHelpers.GenerateArray(N, A, B).ToList();
            var player1 = new ComputerPlayer(ConsoleColor.Green, new MinMaxStrategy(K, MinMaxDepth));
            var player2 = new ComputerPlayer(ConsoleColor.Red, new ImprovedRandomStrategy(K));

            BattlesImplementation(player1, player2, boardValues);

            Assert.IsTrue(true);
        }

        /// <summary>
        /// _ vs NaiveStrategy
        /// </summary>

        [TestMethod]
        public void RandomStrategy_vs_NaiveStrategy()
        {

            var boardValues = GameHelpers.GenerateArray(N, A, B).ToList();
            var player1 = new ComputerPlayer(ConsoleColor.Green, new RandomStrategy());
            var player2 = new ComputerPlayer(ConsoleColor.Red, new NaiveStrategy(boardValues, K));

            BattlesImplementation(player1, player2, boardValues);

            Assert.IsTrue(true);
        }

        [TestMethod]
        public void ImprovedStrategy_vs_NaiveStrategy()
        {
            var boardValues = GameHelpers.GenerateArray(N, A, B).ToList();
            var player1 = new ComputerPlayer(ConsoleColor.Green, new ImprovedRandomStrategy(K));
            var player2 = new ComputerPlayer(ConsoleColor.Red, new NaiveStrategy(boardValues, K));

            BattlesImplementation(player1, player2, boardValues);

            Assert.IsTrue(true);
        }

        [TestMethod]
        public void MinMaxStrategy_vs_NaiveStrategy()
        {
            var boardValues = GameHelpers.GenerateArray(N, A, B).ToList();
            var player1 = new ComputerPlayer(ConsoleColor.Green, new MinMaxStrategy(K, MinMaxDepth));
            var player2 = new ComputerPlayer(ConsoleColor.Red, new NaiveStrategy(boardValues, K));

            BattlesImplementation(player1, player2, boardValues);

            Assert.IsTrue(true);
        }


        /// <summary>
        /// _ vs MinMaxStrategy
        /// </summary>

        [TestMethod]
        public void RandomStrategy_vs_MinMaxStrategy()
        {

            var boardValues = GameHelpers.GenerateArray(N, A, B).ToList();
            var player1 = new ComputerPlayer(ConsoleColor.Green, new RandomStrategy());
            var player2 = new ComputerPlayer(ConsoleColor.Red, new MinMaxStrategy(K, MinMaxDepth));

            BattlesImplementation(player1, player2, boardValues);

            Assert.IsTrue(true);
        }

        [TestMethod]
        public void ImprovedStrategy_vs_MinMaxStrategy()
        {
            var boardValues = GameHelpers.GenerateArray(N, A, B).ToList();
            var player1 = new ComputerPlayer(ConsoleColor.Green, new ImprovedRandomStrategy(K));
            var player2 = new ComputerPlayer(ConsoleColor.Red, new MinMaxStrategy(K, MinMaxDepth));

            BattlesImplementation(player1, player2, boardValues);

            Assert.IsTrue(true);
        }

        [TestMethod]
        public void NaiveStrategy_vs_MinMaxStrategy()
        {
            var boardValues = GameHelpers.GenerateArray(N, A, B).ToList();
            var player1 = new ComputerPlayer(ConsoleColor.Green, new NaiveStrategy(boardValues, K));
            var player2 = new ComputerPlayer(ConsoleColor.Red, new MinMaxStrategy(K, MinMaxDepth));
            

            BattlesImplementation(player1, player2, boardValues);

            Assert.IsTrue(true);
        }
    }
}
