using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ScorchServer.Tests
{
    using System.Collections.Generic;
    using System.Diagnostics;

    using ScorchEngine.Models;

    using SimpleJSON;

    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            string json =
                "[{\"PlayerStates\":[{\"LastUpdateTime\":\"0001-01-01T00:00:00\",\"Id\":0,\"IsReady\":false,\"AngleHorizontal\":0.0,\"AngleVertical\":0.0,\"Force\":0.0},{\"LastUpdateTime\":\"0001-01-01T00:00:00\",\"Id\":0,\"IsReady\":false,\"AngleHorizontal\":0.0,\"AngleVertical\":0.0,\"Force\":0.0}],\"Name\":\"Game0\",\"Id\":\"id0\",\"MaxPlayers\":2,\"Players\":[]},{\"PlayerStates\":[{\"LastUpdateTime\":\"0001-01-01T00:00:00\",\"Id\":0,\"IsReady\":false,\"AngleHorizontal\":0.0,\"AngleVertical\":0.0,\"Force\":0.0},{\"LastUpdateTime\":\"0001-01-01T00:00:00\",\"Id\":0,\"IsReady\":false,\"AngleHorizontal\":0.0,\"AngleVertical\":0.0,\"Force\":0.0}],\"Name\":\"Game0\",\"Id\":\"id0\",\"MaxPlayers\":2,\"Players\":[{\"Name\":\"ido\",\"Id\":\"ido\"]}]";
            
            List<GameInfo> g = GameInfo.ParseJsonAsList(json);

            g.ForEach(g1 => {
                Debug.WriteLine(g1);
                foreach (PlayerInfo player in g1.Players)
                {
                    Debug.WriteLine(player);
                }
            });
        }
    }
}
