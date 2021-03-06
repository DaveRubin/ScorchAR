﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ScorchServer.Db
{
    using Models;

    using ScorchEngine.Models;

    public static class GamesContext
    {
        static volatile Dictionary<string, ServerGame> s_Instance;

        static object s_LockObj = new object();

        static GamesContext()
        {
        }

        public static Dictionary<string, ServerGame> Instance
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
                                s_Instance = new Dictionary<string, ServerGame>();
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

        public static Dictionary<string, ServerGame> Reset()
        {
            lock (s_LockObj)
            {
                try
                {
                    s_Instance = new Dictionary<string, ServerGame>();
                }
                catch (Exception exception)
                {
                    throw new Exception(null, exception);
                }
            }

            return Instance;
        }
    }
}