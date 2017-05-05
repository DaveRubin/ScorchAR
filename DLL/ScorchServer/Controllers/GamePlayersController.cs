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

    public class GamePlayersController : ApiController
    {

        private readonly GamesRepository gamesRepository = new GamesRepository();

        [Route(ServerRoutes.AddPlayerToGameApiUrl)]
        [HttpPost]
        public void AddPlayer(string id, [FromBody] PlayerInfo playerInfo)
        {
            GameInfo game = gamesRepository.GetGame(id);
            game.AddPlayer(playerInfo);
            gamesRepository.Update(game);
        }
    }
}
