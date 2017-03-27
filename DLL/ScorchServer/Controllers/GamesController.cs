using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ScorchServer.Controllers
{
    using ScorchEngine.Models;

    using ScorchServer.Db;

    public class GamesController : ApiController
    {
        private readonly GamesRepository r_GamesRepository = new GamesRepository();



        // GET api/Games
        public IEnumerable<GameInfo> Get()
        {
            return r_GamesRepository.Games.Values;
        }

        // GET api/<controller>/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<controller>
        public void Post([FromBody]string value)
        {
        }

        // PUT api/<controller>/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/<controller>/5
        public void Delete(int id)
        {
        }
    }
}