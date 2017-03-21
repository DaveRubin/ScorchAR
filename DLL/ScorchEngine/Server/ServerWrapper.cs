using System;
using System.Collections.Generic;

namespace ScorchEngine.Server
{
    public class ServerWrapper
    {
        private static int debugCounter = 0;


        /// <summary>
        /// </summary>
        /// <returns></returns>
        public static void GetState(Action<List<PlayerState>> onComplete)
        {
            List<PlayerState> list = new List<PlayerState>
            {
                new PlayerState(){ID = 0,Force = debugCounter/0.1f,AngleHorizontal = debugCounter,AngleVertical = debugCounter},
                new PlayerState(){ID = 1,Force = -debugCounter/0.1f,AngleHorizontal = -debugCounter,AngleVertical = -debugCounter},
            };

            if (onComplete != null)
            {
                onComplete(list);
            }
            debugCounter++;
            if (debugCounter == 100)
            {
                debugCounter = 0;
            }
        }
    }
}