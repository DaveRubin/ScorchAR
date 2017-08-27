using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ScorchServer.Maintenance
{
    using System.Threading.Tasks;

    using Quartz;

    using ScorchServer.Db;

    public class GameCleanupJob: IJob
    {
        public void Execute(IJobExecutionContext context)
        {
            GamesRepository games = new GamesRepository();
            games.Cleanup();
        }

    }
}