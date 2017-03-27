using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ScorchServer.Controllers
{
    using ScorchEngine.Models;
    using ScorchEngine.Server;

    using ScorchServer.Db;

    public class GamesController : ApiController
    {
        private readonly GamesRepository r_GamesRepository = new GamesRepository();


        [Route(ServerRoutes.GetGamesApiUrl)]
        public IEnumerable<GameInfo> Get()
        {
            return r_GamesRepository.Games.Values;
        }

        [Route(ServerRoutes.GetGameApiUrl)]
        public GameInfo Get(string i_Id)
        {
            return r_GamesRepository.GetGame(i_Id);
        }

        // POST api/Games
        public void Post([FromBody]GameInfo i_GameInfo)
        {
            r_GamesRepository.AddGame(i_GameInfo);
        }

        [Route(ServerRoutes.AddPlayerToGameApiUrl)]
        public void  AddPlayer(string i_Id, [FromBody]PlayerInfo i_PlayerInfo)
        {
            r_GamesRepository.GetGame(i_Id).Players.Add(i_PlayerInfo);
        }
    }
}