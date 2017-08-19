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
        private UserController controller = new UserController();
        private PlayerInfo testUser = new PlayerInfo() {Id="0",Name = "Dummy User"};
        private PlayerInfo resultUser;
        [TestMethod]
         public void ControllerTest()
         {

            controller.Post(testUser);
            resultUser = controller.Get(testUser.Id);
            Debug.WriteLine(resultUser);
         }
    }
}
