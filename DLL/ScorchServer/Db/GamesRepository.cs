using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ScorchServer.Db
{
    using ScorchEngine.Models;

    public class GamesRepository : IGamesRepository
    {
        private Dictionary<string, GameInfo> games = GamesContext.Instance;

        public void AddGame(GameInfo gameInfo)
        {
            games.Add(gameInfo.Id, gameInfo);
        }

        public GameInfo GetGame(string id)
        {
            // TODO LOG
            GameInfo result = games.ContainsKey(id) ? games[id] : new GameInfo { Id = "YOU SHOULD NOT BE HERE" };

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
            games.Add(game.Id, game);
        }
    }
}