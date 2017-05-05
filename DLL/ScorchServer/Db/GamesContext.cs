using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ScorchServer.Db
{
    using ScorchEngine.Models;

    public static class GamesContext
    {
        static volatile Dictionary<string, GameInfo> s_Instance;
        static object s_LockObj = new object();

        static GamesContext()
        {
        }

        public static Dictionary<string, GameInfo> Instance
        {
            get
            {
                if (s_Instance == null)
                    lock (s_LockObj)
                    {
                        if (s_Instance == null)
                        {
                            try
                            {
                                s_Instance = new Dictionary<string, GameInfo>();

                                for (int i = 0; i < 6; i++)
                                {
                                    GameInfo gameInfo = new GameInfo
                                    {
                                        MaxPlayers = new Random().Next(1, 4),
                                        Name = "Games " + i,
                                        Id = "id" + i
                                    };
                                    s_Instance.Add(gameInfo.Id, gameInfo);
                                }

                            }
                            catch (Exception exception)
                            {
                                throw new Exception(null, exception);
                            }
                        }
                    }

                return s_Instance;
            }
        }
    }
}