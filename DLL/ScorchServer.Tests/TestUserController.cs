using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ScorchServer.Tests
{
    using System.Diagnostics;
    using System.Linq;
    using System.Net.Http;

    using ScorchServer.Controllers;
    using ScorchServer.Models;

    [TestClass]
    public class TestUserController
    {
        private UserController m_Controller = new UserController();
        private User m_TestUser = new User("AAAA") { FirstName = "Dushi", LastName = "Ben-Dushon" };
        private User m_ResultUser;
        /* [TestMethod]
         public void ControllerTest()
         {

             m_Controller.Post(m_TestUser);
             m_ResultUser = m_Controller.Get(m_TestUser.Id);
             Assert.AreEqual(m_TestUser,m_ResultUser);
             m_TestUser.Money = 2000;
             m_Controller.Put(m_TestUser.Id,m_TestUser);
             m_ResultUser = m_Controller.Get(m_TestUser.Id);
             Assert.AreEqual(m_TestUser, m_ResultUser);
             m_Controller.Delete(m_TestUser.Id);
             Assert.AreEqual(0, m_Controller.Get().Count());
         }*/
    }
}
