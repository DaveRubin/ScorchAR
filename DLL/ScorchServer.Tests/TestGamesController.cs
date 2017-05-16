﻿using System;

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
            PlayerInfo playerInfo = new PlayerInfo { Id = "AAAA", Name = "Dushi" };
            List<GameInfo> games = client.GetGames();
            string gameId = games[0].Id;
            client.AddPlayerToGame(gameId, playerInfo);
            GameInfo gameInfo = client.GetGame(gameId);
            foreach (PlayerInfo player in gameInfo.Players)
            {
                Debug.WriteLine(player);
            }
        }
    }
}