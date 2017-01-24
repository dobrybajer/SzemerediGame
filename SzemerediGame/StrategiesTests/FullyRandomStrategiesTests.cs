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
    // Testy z tworzniem nowych plansz
    public class FullyRandomStrategiesTests
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

        private void BattlesImplementation(Func<Game> gameCreator, Type StrategyTypePlayer1, Type StrategyTypePlayer2)
        {
            
            var resultList = new List<GameResult>();

            //Act
            for (var i = 0; i < GamesIterations; i++)
            {
                var game = gameCreator.Invoke(); 
                var result = game.Start();
                resultList.Add(result);
            }

            //Assert
            Console.WriteLine("Ties: " + resultList.Count(x => x.GameState == GameState.Tie));
            Console.WriteLine(StrategyTypePlayer1.Name + ": " + resultList.Count(x => x.GameState == GameState.Win && x.Winner.StrategyName == StrategyTypePlayer1.Name));
            Console.WriteLine(StrategyTypePlayer2.Name + ": " + resultList.Count(x => x.GameState == GameState.Win && x.Winner.StrategyName == StrategyTypePlayer2.Name));
        }

        /// <summary>
        /// _ vs RandomStrategy
        /// </summary>

        [TestMethod]
        public void ImprovedRandomStrategy_vs_RandomStrategy()
        {
            BattlesImplementation(() =>
            {
                var boardValues = GameHelpers.GenerateArray(N, A, B).ToList();
                var player1 = new ComputerPlayer(ConsoleColor.Green, new ImprovedRandomStrategy(K));
                var player2 = new ComputerPlayer(ConsoleColor.Red, new RandomStrategy());
                return new Game(player1,player2,boardValues,K);

            }, typeof(ImprovedRandomStrategy), typeof(RandomStrategy));

            Assert.IsTrue(true);
        }


        [TestMethod]
        public void NaiveStrategy_vs_RandomStrategy()
        {
            BattlesImplementation(() =>
            {
                var boardValues = GameHelpers.GenerateArray(N, A, B).ToList();
                var player1 = new ComputerPlayer(ConsoleColor.Green, new NaiveStrategy(boardValues, K));
                var player2 = new ComputerPlayer(ConsoleColor.Red, new RandomStrategy());
                return new Game(player1, player2, boardValues, K);

            }, typeof(NaiveStrategy), typeof(RandomStrategy));

            Assert.IsTrue(true);
        }


        [TestMethod]
        public void MinMaxStrategy_vs_RandomStrategy()
        {
            BattlesImplementation(() =>
            {
                var boardValues = GameHelpers.GenerateArray(N, A, B).ToList();
                var player1 = new ComputerPlayer(ConsoleColor.Green, new MinMaxStrategy(K, MinMaxDepth));
                var player2 = new ComputerPlayer(ConsoleColor.Red, new RandomStrategy());
                return new Game(player1, player2, boardValues, K);

            }, typeof(MinMaxStrategy), typeof(RandomStrategy));

            Assert.IsTrue(true);
        }

        ///// <summary>
        ///// _ vs ImprovedRandomStrategy
        ///// </summary>

        [TestMethod]
        public void RandomStrategy_vs_ImprovedRandomStrategy()
        {
            BattlesImplementation(() =>
            {
                var boardValues = GameHelpers.GenerateArray(N, A, B).ToList();
                var player1 = new ComputerPlayer(ConsoleColor.Green, new RandomStrategy());
                var player2 = new ComputerPlayer(ConsoleColor.Red, new ImprovedRandomStrategy(K));
                return new Game(player1, player2, boardValues, K);

            }, typeof(RandomStrategy), typeof(ImprovedRandomStrategy));

            Assert.IsTrue(true);
        }

        [TestMethod]
        public void NaiveStrategy_vs_ImprovedRandomStrategy()
        {
            BattlesImplementation(() =>
            {
                var boardValues = GameHelpers.GenerateArray(N, A, B).ToList();
                var player1 = new ComputerPlayer(ConsoleColor.Green, new NaiveStrategy(boardValues, K));
                var player2 = new ComputerPlayer(ConsoleColor.Red, new ImprovedRandomStrategy(K));
                return new Game(player1, player2, boardValues, K);

            }, typeof(NaiveStrategy), typeof(ImprovedRandomStrategy));

            Assert.IsTrue(true);
        }

        [TestMethod]
        public void MinMaxStrategy_vs_ImprovedRandomStrategy()
        {            
            BattlesImplementation(() =>
            {
                var boardValues = GameHelpers.GenerateArray(N, A, B).ToList();
                var player1 = new ComputerPlayer(ConsoleColor.Green, new MinMaxStrategy(K, MinMaxDepth));
                var player2 = new ComputerPlayer(ConsoleColor.Red, new ImprovedRandomStrategy(K));
                return new Game(player1, player2, boardValues, K);

            }, typeof(MinMaxStrategy), typeof(ImprovedRandomStrategy));

            Assert.IsTrue(true);
        }

        ///// <summary>
        ///// _ vs NaiveStrategy
        ///// </summary>

        [TestMethod]
        public void RandomStrategy_vs_NaiveStrategy()
        {
            BattlesImplementation(() =>
            {
                var boardValues = GameHelpers.GenerateArray(N, A, B).ToList();
                var player1 = new ComputerPlayer(ConsoleColor.Green, new RandomStrategy());
                var player2 = new ComputerPlayer(ConsoleColor.Red, new NaiveStrategy(boardValues, K));
                return new Game(player1, player2, boardValues, K);

            }, typeof(RandomStrategy), typeof(NaiveStrategy));

            Assert.IsTrue(true);
        }

        [TestMethod]
        public void ImprovedStrategy_vs_NaiveStrategy()
        {
            BattlesImplementation(() =>
            {
                var boardValues = GameHelpers.GenerateArray(N, A, B).ToList();
                var player1 = new ComputerPlayer(ConsoleColor.Green, new ImprovedRandomStrategy(K));
                var player2 = new ComputerPlayer(ConsoleColor.Red, new NaiveStrategy(boardValues, K));
                return new Game(player1, player2, boardValues, K);

            }, typeof(ImprovedRandomStrategy), typeof(NaiveStrategy));

            Assert.IsTrue(true);
        }

        [TestMethod]
        public void MinMaxStrategy_vs_NaiveStrategy()
        {
            BattlesImplementation(() =>
            {
                var boardValues = GameHelpers.GenerateArray(N, A, B).ToList();
                var player1 = new ComputerPlayer(ConsoleColor.Green, new MinMaxStrategy(K, MinMaxDepth));
                var player2 = new ComputerPlayer(ConsoleColor.Red, new NaiveStrategy(boardValues, K));
                return new Game(player1, player2, boardValues, K);

            }, typeof(MinMaxStrategy), typeof(NaiveStrategy));

            Assert.IsTrue(true);
        }


        ///// <summary>
        ///// _ vs MinMaxStrategy
        ///// </summary>

        [TestMethod]
        public void RandomStrategy_vs_MinMaxStrategy()
        {
            BattlesImplementation(() =>
            {
                var boardValues = GameHelpers.GenerateArray(N, A, B).ToList();
                var player1 = new ComputerPlayer(ConsoleColor.Green, new RandomStrategy());
                var player2 = new ComputerPlayer(ConsoleColor.Red, new MinMaxStrategy(K, MinMaxDepth));
                return new Game(player1, player2, boardValues, K);

            }, typeof(RandomStrategy), typeof(MinMaxStrategy));

            Assert.IsTrue(true);
        }

        [TestMethod]
        public void ImprovedStrategy_vs_MinMaxStrategy()
        {
            BattlesImplementation(() =>
            {
                var boardValues = GameHelpers.GenerateArray(N, A, B).ToList();
                var player1 = new ComputerPlayer(ConsoleColor.Green, new ImprovedRandomStrategy(K));
                var player2 = new ComputerPlayer(ConsoleColor.Red, new MinMaxStrategy(K, MinMaxDepth));
                return new Game(player1, player2, boardValues, K);

            }, typeof(ImprovedRandomStrategy), typeof(MinMaxStrategy));

            Assert.IsTrue(true);
        }

        [TestMethod]
        public void NaiveStrategy_vs_MinMaxStrategy()
        {
            BattlesImplementation(() =>
            {
                var boardValues = GameHelpers.GenerateArray(N, A, B).ToList();
                var player1 = new ComputerPlayer(ConsoleColor.Green, new NaiveStrategy(boardValues, K));
                var player2 = new ComputerPlayer(ConsoleColor.Red, new MinMaxStrategy(K, MinMaxDepth));
                return new Game(player1, player2, boardValues, K);

            }, typeof(NaiveStrategy), typeof(MinMaxStrategy));

            Assert.IsTrue(true);
        }
    }
}
