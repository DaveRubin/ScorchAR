using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ScorchEngine.Models
{
    using ScorchEngine.Server;

    public class PollResult
    {
        public List<PlayerState> PlayerStates { get; set; }

        public int RoundWinnerIndex { get; set; }
    }
}
