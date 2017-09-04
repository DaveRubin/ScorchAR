using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ScorchServer.Tests
{
    using System.Diagnostics;
    using System.Linq;
    using System.Net.Http;

    using ScorchEngine.Models;

    using ScorchServer.Controllers;
    using ScorchServer.Models;

    [TestClass]
    public class TestUserController
    {
        
        [TestMethod]
         public void ControllerTest()
         {

            DateTime now = DateTime.Now;
            System.Threading.Thread.Sleep(1000);
            DateTime Later = DateTime.Now;
            Debug.WriteLine((Later - now).TotalMilliseconds);
         }
    }
}
