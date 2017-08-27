﻿using System;
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
            PlayerStates = new List<ServerPlayerState>();
            for (int i = 0; i < MaxPlayers; ++i)
            {
                PlayerStates.Add( new ServerPlayerState());
            }
            LastUpdateTime = DateTime.Now;
        }

        public void Update(GameInfo gameInfo)
        {
            Id = gameInfo.Id;
            Name = gameInfo.Name;
            MaxPlayers = gameInfo.MaxPlayers;
            Players = gameInfo.Players;
            LastUpdateTime = DateTime.Now;
        }
    }
}