using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ScorchServer.Models
{
    using ScorchEngine.Server;

    public class ServerPlayerState : PlayerState
    {
        public DateTime LastUpdateTime { get; set; }
        

        public ServerPlayerState(PlayerState state)
        {
            Id = state.Id;
            IsReady = state.IsReady;
            AngleHorizontal = state.AngleHorizontal;
            AngleVertical = state.AngleVertical;
            Force = state.Force;
            PositionX = state.PositionX;
            PositionY = state.PositionY;
            PositionZ = state.PositionZ;
            LastUpdateTime = DateTime.Now;
            IsActive = true;
            IsValid = false;
        }

        public ServerPlayerState()
        {
            IsActive = true;
            IsValid = false;
        }

        public void Update(PlayerState state)
        {
            Id = state.Id;
            IsReady = state.IsReady;
            AngleHorizontal = state.AngleHorizontal;
            AngleVertical = state.AngleVertical;
            Force = state.Force;
            PositionX = state.PositionX;
            PositionY = state.PositionY;
            PositionZ = state.PositionZ;
            LastUpdateTime = DateTime.Now;
            IsValid = true;
        }

        
    }
}