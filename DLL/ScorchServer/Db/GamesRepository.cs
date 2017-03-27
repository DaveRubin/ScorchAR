using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ScorchServer.Db
{
    using ScorchEngine.Models;

    public class GamesRepository
    {
        public Dictionary<string, GameInfo> Games { get; }

        public GamesRepository()
        {
            Games = new Dictionary<string, GameInfo>();
            for (int i = 0; i < 6; i++)
            {
                GameInfo gameInfo = new GameInfo();
                gameInfo.MaxPlayers = new Random().Next(1, 4);
                gameInfo.Name = "Games " + i;
                gameInfo.ID = "id" + i;
                Games.Add(gameInfo.ID,gameInfo);
            }
        }

        public void AddGame(GameInfo i_GameInfo)
        {
            Games.Add(i_GameInfo.ID,i_GameInfo);
        }

        public GameInfo GetGame(string i_Id)
        {
            GameInfo result;
            //TODO LOG
            Games.TryGetValue(i_Id, out result);
            return result;
        }

        public void RemoveGame(string i_Id)
        {
            Games.Remove(i_Id);
        }
    }
}