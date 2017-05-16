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
    using ScorchServer.Models;

    public class GamePlayersController : ApiController
    {
        private readonly GamesRepository gamesRepository = new GamesRepository();

        [Route(ServerRoutes.AddPlayerToGameApiUrl)]
        [HttpPost]
        public int AddPlayer(string id, [FromBody] PlayerInfo playerInfo)
        {
            int newPlayerIndex = -1;
            GameInfo game = gamesRepository.GetGame(id);
            game.AddPlayer(playerInfo, ref newPlayerIndex);

            gamesRepository.Update(game);
            return newPlayerIndex;
        }

        [Route(ServerRoutes.UpdatePlayerStateUrl)]
        [HttpPut]
        public List<PlayerState> UpdatePlayerStates(string id, [FromBody] PlayerState playerState)
        {
            ServerGame game = gamesRepository.GetGame(id);
            game.PlayerStates[playerState.Id].Update(playerState);

            gamesRepository.Update(game);
            return game.PlayerStates.Cast<PlayerState>().ToList();
        }
    }
}