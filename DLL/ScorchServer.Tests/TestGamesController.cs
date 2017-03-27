using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ScorchServer.Tests
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;

    using RestSharp;

    using ScorchEngine.Models;

    [TestClass]
    public class TestGamesController
    {
        [TestMethod]
        public void TestMethod1()
        {
            RestClient client = new RestClient("http://scorchar.azurewebsites.net");
            RestRequest request = new RestRequest("api/Games",Method.GET);
            List<GameInfo> result = client.Execute<List<GameInfo>>(request).Data;
            foreach (GameInfo gameInfo in result)
            {
                Debug.WriteLine(gameInfo.ID);
            }
        }
    }
}
