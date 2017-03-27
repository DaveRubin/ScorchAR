using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ScorchEngine.Server
{
    public static class ServerRoutes
    {
        public const string GetGamesApiUrl = "api/Games";
        public const string AddPlayerToGameApiUrl = "api/Games/{id}/addPlayer";
        public const string GetGameApiUrl = "api/Games/{id}";

        public const string ServerBaseUri = "http://scorchar.azurewebsites.net";
    }
}
