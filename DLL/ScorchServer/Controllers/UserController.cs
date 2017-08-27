using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ScorchServer.Controllers
{
    using MongoDB.Driver;

    using ScorchEngine.Models;
    using ScorchEngine.Server;

    using ScorchServer.Db;
    using ScorchServer.Models;

    public class UserController : ApiController
    {
        private readonly UserRepository userRepository = new UserRepository();

        private readonly object createLockObject = new object();


        [Route(ServerRoutes.CreateUserUrl)]
        [HttpPost]
        public PlayerInfo CreateUer([FromBody] string name)
        {
            PlayerInfo newUser;
            lock (createLockObject)
            {
                string id = userRepository.GetNumberOfUsers().ToString();
                newUser = new PlayerInfo() { Id = id, Name = name };
                userRepository.Insert(newUser);
            }

            return newUser;
        }

        [Route(ServerRoutes.UpdateUserNameUrl)]
        [HttpPost]
        public PlayerInfo UpdateUserName([FromBody] PlayerInfo playerInfo)
        {
            userRepository.Update(playerInfo);
            return playerInfo;
        }
    }
}