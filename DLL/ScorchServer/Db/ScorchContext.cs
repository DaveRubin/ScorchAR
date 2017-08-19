using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ScorchServer.Db
{
    using MongoDB.Driver;

    public static class ScorchContext
    {
        static volatile IMongoDatabase s_Instance;
        static object s_LockObj = new object();

        static ScorchContext()
        {
        }

        public static IMongoDatabase Instance
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
                                MongoClient client = new MongoClient(ServerSettings.ConnectionString);
                                s_Instance = client.GetDatabase(ServerSettings.k_DbName);


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