using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ScorchServer.Models
{
    using ScorchEngine.Server;

    public class ServerPlayerState : PlayerState
    {
        public ServerPlayerState(PlayerState state, DateTime updateTime)
        {
            Id = state.Id;
            IsReady = state.IsReady;
            AngleHorizontal = state.AngleHorizontal;
            AngleVertical = state.AngleVertical;
            Force = state.Force;
            LastUpdateTime = updateTime;
        }

        public ServerPlayerState()
        {
            
        }

        public void Update(PlayerState state)
        {
            Id = state.Id;
            IsReady = state.IsReady;
            AngleHorizontal = state.AngleHorizontal;
            AngleVertical = state.AngleVertical;
            Force = state.Force;
            LastUpdateTime = DateTime.Now;
        }

        public DateTime LastUpdateTime { get; set; }
    }
}