using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ScorchServer.Controllers
{
    using System.Web.Helpers;
    using System.Web.Http.Description;

    using ScorchEngine.Models;
    using ScorchEngine.Server;

    using ScorchServer.Db;

    public class GamesController : ApiController
    {
        private readonly GamesRepository gamesRepository = new GamesRepository();


        [Route(ServerRoutes.GetGamesApiUrl)]
        [HttpGet]
        public IEnumerable<GameInfo> Get()
        {
            return gamesRepository.GetGames();
        }

        [Route(ServerRoutes.GetGameApiUrl)]
        [HttpGet]
        public GameInfo Get(string id)
        {
            GameInfo game = gamesRepository.GetGame(id);
            return game;
        }

        // POST api/Games
        public void Post([FromBody] GameInfo gameInfo)
        {
            gamesRepository.AddGame(gameInfo);
        }

        [Route(ServerRoutes.ClearGamesUrl)]
        [HttpGet]
        public void Clear()
        {
            gamesRepository.Reset();
        }

    }
}