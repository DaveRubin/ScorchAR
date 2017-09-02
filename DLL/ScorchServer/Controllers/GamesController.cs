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

        private static int gameIdSequence = 1;

        private readonly object createLockObject = new object();

        [Route(ServerRoutes.CreateGameUrl)]
        [HttpPost]
        public GameInfo Create([FromBody] PlayerInfo playerInfo)
        {
            string name = string.Empty;
            int maxPlayers = 0;
            string gameId;
            lock (createLockObject)
            {
                ++gameIdSequence;
                gameId = gameIdSequence.ToString();
            }
            
            foreach (KeyValuePair<string, string> parameter in Request.GetQueryNameValuePairs())
            {
                switch (parameter.Key)
                {
                    case "name":
                        name = parameter.Value;
                        break;
                    case "maxPlayers":
                        maxPlayers = int.Parse(parameter.Value);
                        break;
                    default:
                        break;
                }
            }

            GameInfo createdGameContext = new GameInfo { Id = gameId, MaxPlayers = maxPlayers, Name = name };
            int playerIndex = -1;
            createdGameContext.AddPlayer(playerInfo, ref playerIndex);
            gamesRepository.AddGame(createdGameContext);
            return createdGameContext;
        }

        [Route(ServerRoutes.GetGamesApiUrl)]
        [HttpGet]
        public IEnumerable<GameInfo> Get()
        {
            return gamesRepository.GetGames().Select(game => game).Where(game => !game.IsFull);
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

        [Route(ServerRoutes.CancelGame)]
        [HttpGet]
        public void CancelGame(string id)
        {
            gamesRepository.RemoveGame(id);
        }

        [Route(ServerRoutes.NextRound)]
        [HttpGet]
        public GameInfo NextRound(string id)
        {
            GameInfo game = gamesRepository.GetGame(id);
            game.CreatePositionsForPlayers();
            return game;
        }
        
    }
}