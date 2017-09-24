using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ScorchEngine.Models
{
    using ScorchEngine.Server;

    [System.Serializable]
    public class PollResult
    {
        public List<PlayerState> PlayerStates;

        public int RoundWinnerIndex;

        public PollResult()
        {
            
        }
    }
}
