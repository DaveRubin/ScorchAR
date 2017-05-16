using System;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ScorchServer.Tests
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;

    using RestSharp;

    using ScorchEngine.Models;
    using ScorchEngine.Server;

    [TestClass]
    public class TestGamesController
    {
        ScorchServerClient client = new ScorchServerClient();

        [TestMethod]
        public void TestMethod1()
        {
            PlayerInfo player1 = new PlayerInfo { Id = "AAAA", Name = "Dushi" };
            PlayerInfo player2 = new PlayerInfo { Id = "BBBB", Name = "Amidushi" };
            List<GameInfo> games = client.GetGames();
            string gameId = games[0].Id;
            int p1Idx = client.AddPlayerToGame(gameId, player1);
            Debug.WriteLine("player {0} : idx: {1}", player1, p1Idx);
            int p2Idx = client.AddPlayerToGame(gameId, player2);
            Debug.WriteLine("player {0} : idx: {1}", player2, p2Idx);
            GameInfo gameInfo = client.GetGame(gameId);
            foreach (PlayerInfo player in gameInfo.Players)
            {
                Debug.WriteLine(player);
            }

            PlayerState p1State = new PlayerState
                                      {
                                          Id = p1Idx, 
                                          AngleHorizontal = 0.1f, 
                                          AngleVertical = 0.2f, 
                                          Force = 100
                                      };

            PlayerState p2State = new PlayerState
                                      {
                                          Id = p2Idx, 
                                          AngleHorizontal = 0.2f, 
                                          AngleVertical = 0.3f, 
                                          Force = 200
                                      };

            client.UpdatePlayerState(gameId, p1State);

            List<PlayerState> stateList = client.UpdatePlayerState(gameId, p2State);
            printStates(stateList);

            p1State.AngleHorizontal = 0.5f;
            p2State.AngleHorizontal = 0.12f;
            client.UpdatePlayerState(gameId, p1State);

            stateList = client.UpdatePlayerState(gameId, p2State);
            printStates(stateList);

        }

        private void printStates(List<PlayerState> stateList)
        {
            foreach (PlayerState state in stateList)
            {
                Debug.WriteLine(state);
            }
        }
    }
}