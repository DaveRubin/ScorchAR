using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ScorchServer.Db
{
    using ScorchEngine.Models;

    public class GamesRepository : IGamesRepository
    {
        private Dictionary<string, GameInfo> games;

        public GamesRepository()
        {
            games = new Dictionary<string, GameInfo>();
            for (int i = 0; i < 6; i++)
            {
                GameInfo gameInfo = new GameInfo
                                        {
                                            MaxPlayers = new Random().Next(1, 4), 
                                            Name = "Games " + i, 
                                            ID = "id" + i
                                        };
                games.Add(gameInfo.ID, gameInfo);
            }
        }

        public void AddGame(GameInfo gameInfo)
        {
            games.Add(gameInfo.ID, gameInfo);
        }

        public GameInfo GetGame(string id)
        {
            // TODO LOG
            GameInfo result = games.ContainsKey(id) ? games[id] : new GameInfo { ID = "YOU SHOULD NOT BE HERE" };

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
            if (games.ContainsKey(game.ID))
            {
                games.Remove(game.ID);               
            }
            games.Add(game.ID, game);
        }
    }
}