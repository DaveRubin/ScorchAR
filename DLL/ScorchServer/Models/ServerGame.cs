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

        private int endRoundAcknoledgments;

        public void AcknoledgeEndOfRound()
        {
            ++endRoundAcknoledgments;
            if (endRoundAcknoledgments == MaxPlayers)
            {
                endRoundAcknoledgments = 0;
                RoundWinnerIndex = -1;
            }
        }
      

        public ServerGame(GameInfo gameInfo)
        {
            endRoundAcknoledgments = 0;
            RoundWinnerIndex = -1;
            Id = gameInfo.Id;
            Name = gameInfo.Name;
            MaxPlayers = gameInfo.MaxPlayers;
            Players = gameInfo.Players;
            Rounds = gameInfo.Rounds;
            PlayerStates = new List<ServerPlayerState>();
            for (int i = 0; i < MaxPlayers; ++i)
            {
                PlayerStates.Add( new ServerPlayerState());
            }
            PlayerPositions = gameInfo.PlayerPositions;
            DestructableObjectPositions = gameInfo.DestructableObjectPositions;
            LastUpdateTime = DateTime.Now;
        }

        public void Update(GameInfo gameInfo)
        {
            Id = gameInfo.Id;
            Name = gameInfo.Name;
            MaxPlayers = gameInfo.MaxPlayers;
            Players = gameInfo.Players;
            PlayerPositions = gameInfo.PlayerPositions;
            DestructableObjectPositions = gameInfo.DestructableObjectPositions;
            LastUpdateTime = DateTime.Now;
            Rounds = gameInfo.Rounds;
        }
    }
}