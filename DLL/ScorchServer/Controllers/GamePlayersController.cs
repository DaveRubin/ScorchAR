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
            ServerGame game = gamesRepository.GetGame(id);
            game.AddPlayer(playerInfo, ref newPlayerIndex);
            game.LastUpdateTime = DateTime.Now;
            return newPlayerIndex;
        }

        [Route(ServerRoutes.UpdatePlayerStateUrl)]
        [HttpPost]
        public PollResult UpdatePlayerStates(string id, [FromBody] PlayerState playerState)
        {
            ServerGame game = gamesRepository.GetGame(id);
            PollResult pollResult = new PollResult();
            game.PlayerStates[playerState.Id].Update(playerState);
            game.LastUpdateTime = DateTime.Now;
            pollResult.PlayerStates = game.PlayerStates.Cast<PlayerState>().ToList();
            pollResult.RoundWinnerIndex = game.RoundWinnerIndex;
            return pollResult;

        }

        // ONLY FOR DEBUG
        [Route(ServerRoutes.SetPlayerInActiveUrl)]
        [HttpPost]
        public void RemovePlayerFromGame(string id, int index)
        {
            ServerGame game = gamesRepository.GetGame(id);
            game.PlayerStates[index].IsActive = false;

            for(int i =0 ; i < game.PlayerStates.Count ; ++i)
            {
                if (game.PlayerStates[i].IsActive)
                {
                    return;
                }
            }
            //TODO write to db
            gamesRepository.RemoveGame(id);
        }

        [Route(ServerRoutes.GameRoundWinner)]
        [HttpGet]
        public void SetGameRoundWinner(string id, int index)
        { 
            gamesRepository.GetGame(id).RoundWinnerIndex = index;
        }

        [Route(ServerRoutes.GameEndRound)]
        [HttpGet]
        public void SetGameEndRound(string id)
        {
            gamesRepository.GetGame(id).AcknoledgeEndOfRound();
        }
    }
}