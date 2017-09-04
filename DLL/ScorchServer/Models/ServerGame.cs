using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ScorchServer.Models
{
    using ScorchEngine.Models;
    using ScorchEngine.Server;

    public class ServerGame : GameInfo
    {

        public List<ServerPlayerState> PlayerStates { get; set; }
        public DateTime LastUpdateTime { get; set; }

      

        public ServerGame(GameInfo gameInfo)
        {
            Id = gameInfo.Id;
            Name = gameInfo.Name;
            MaxPlayers = gameInfo.MaxPlayers;
            Players = gameInfo.Players;
            Rounds = gameInfo.Rounds;
            PlayerStates = new List<ServerPlayerState>();
            PlayerPositions = gameInfo.PlayerPositions;
            LastUpdateTime = DateTime.Now;
        }

        public void Update(GameInfo gameInfo)
        {
            Id = gameInfo.Id;
            Name = gameInfo.Name;
            MaxPlayers = gameInfo.MaxPlayers;
            Players = gameInfo.Players;
            PlayerPositions = gameInfo.PlayerPositions;
            LastUpdateTime = DateTime.Now;
            Rounds = gameInfo.Rounds;
        }
    }
}