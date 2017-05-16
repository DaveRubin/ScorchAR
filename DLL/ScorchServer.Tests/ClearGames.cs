using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ScorchServer.Tests
{
    using ScorchEngine.Server;

    [TestClass]
    public class ClearGames
    {
        [TestMethod]
        public void TestMethod1()
        {
            ScorchServerClient client = new ScorchServerClient();
            client.ResetGames();
        }
    }
}
