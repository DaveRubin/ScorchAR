using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ScorchServer.Controllers
{
    using MongoDB.Driver;

    using ScorchServer.Db;
    using ScorchServer.Models;

    public class UserController : ApiController
    {
        private readonly UserRepository r_UserRepository = new UserRepository();


        // GET: api/User
        public IEnumerable<User> Get()
        {
            return r_UserRepository.Users.AsQueryable().ToEnumerable();
        }

        // GET: api/User/dfsafds
        public User Get(string i_Id)
        {
            return r_UserRepository.Find(i_Id);
        }

        // POST: api/User
        public void Post([FromBody] User i_User)
        {
            r_UserRepository.Insert(i_User);
        }

        // PUT: api/User/5
        public void Put(string i_Id, [FromBody] User i_User)
        {
            r_UserRepository.Update(i_User);
        }

        // DELETE: api/User/5
        public void Delete(string i_Id)
        {
            r_UserRepository.Delete(i_Id);
        }
    }
}