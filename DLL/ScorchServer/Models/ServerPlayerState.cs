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
    }
}