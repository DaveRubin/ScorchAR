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

        public ServerGame(GameInfo gameInfo)
        {
            Id = gameInfo.Id;
            Name = gameInfo.Name;
            MaxPlayers = gameInfo.MaxPlayers;
            PlayerStates = new List<ServerPlayerState>();
        }
    }
}