using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScorchServer.Db
{
    using ScorchEngine.Models;

    using ScorchServer.Models;

    public interface IGamesRepository
    {
        void AddGame(GameInfo gameInfo);

        ServerGame GetGame(string id);

        void RemoveGame(string id);

        IEnumerable<GameInfo> GetGames();

        void Update(GameInfo game);
    }
}