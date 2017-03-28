using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScorchServer.Db
{
    using ScorchEngine.Models;

    public interface IGamesRepository
    {
        void AddGame(GameInfo gameInfo);

        GameInfo GetGame(string id);

        void RemoveGame(string id);

        IEnumerable<GameInfo> GetGames();

        void Update(GameInfo game);
    }
}