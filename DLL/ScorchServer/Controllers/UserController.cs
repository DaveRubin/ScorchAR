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

        private static int gameIdSequence = 1;
        private readonly object createLockObject = new object();

        [Route(ServerRoutes.CreateUserUrl)]
        [HttpPost]
        public PlayerInfo CreateUer(string name)
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

        /*  // GET: api/User
          public IEnumerable<User> Get()
          {
             // return userRepository.Users.AsQueryable().ToEnumerable();
          }*/

        // GET: api/User/dfsafds
        public PlayerInfo Get(string i_Id)
        {
            return userRepository.Find(i_Id);
        }

        // POST: api/User
        public void Post([FromBody] PlayerInfo i_User)
        {
            userRepository.Insert(i_User);
        }

        // PUT: api/User/5
        public void Put(string i_Id, [FromBody] PlayerInfo i_User)
        {
            userRepository.Update(i_User);
        }

        // DELETE: api/User/5
        public void Delete(string i_Id)
        {
            userRepository.Delete(i_Id);
        }
    }
}