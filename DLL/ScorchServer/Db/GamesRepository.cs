using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ScorchServer.Db
{
    using ScorchEngine;
    using ScorchEngine.Models;

    using ScorchServer.Models;

    public class GamesRepository : IGamesRepository
    {
        private Dictionary<string, ServerGame> games = GamesContext.Instance;

        public void AddGame(GameInfo gameInfo)
        {
            games.Add(gameInfo.Id, new ServerGame(gameInfo));
        }

        public GameInfo GetGame(string id)
        {
            // TODO LOG
            GameInfo result = games.ContainsKey(id) ? games[id] :null;

            return result;
        }

        public void RemoveGame(string id)
        {
            games.Remove(id);
        }

        public IEnumerable<GameInfo> GetGames()
        {
            return games.Values;
        }

        public void Update(GameInfo game)
        {
            if (games.ContainsKey(game.Id))
            {
                games.Remove(game.Id);               
            }
            games.Add(game.Id, new ServerGame(game));
        }
    }
}