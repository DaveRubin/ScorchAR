using System.Collections.Generic;

using ScorchEngine.Models;

using ScorchServer.Models;

namespace ScorchServer.Db
{
    using System;
    using System.Linq;

    public class GamesRepository : IGamesRepository
    {
        private Dictionary<string, ServerGame> games = GamesContext.Instance;

        private readonly object gamesLock = new object();

        public void AddGame(GameInfo gameInfo)
        {
            lock (gamesLock)
            {
                games.Add(gameInfo.Id, new ServerGame(gameInfo));
            }
        }

        public ServerGame GetGame(string id)
        {
            ServerGame result;
            lock (gamesLock)
            {
                result = games.ContainsKey(id) ? games[id] : null;
            }

            return result;
        }

        public void RemoveGame(string id)
        {
            lock (gamesLock)
            {
                games.Remove(id);
            }
        }

        public IEnumerable<GameInfo> GetGames()
        {
            return games.Values;
        }

        public void Update(GameInfo game)
        {
            lock (gamesLock)
            {
                if (games.ContainsKey(game.Id))
                {
                    games[game.Id].Update(game);
                }
            }
        }

        public void Reset()
        {
            lock (gamesLock)
            {
                games = GamesContext.Reset();
            }
        }

        public void Cleanup()
        {
            lock (gamesLock)
            {
                List<string> gamesNames = games.Keys.ToList();
                foreach (string name in gamesNames)
                {
                    if (games[name].LastUpdateTime < DateTime.Now.AddMinutes(-2))
                    {
                        games.Remove(name);
                    }   
                }
            }
        }
    }
}