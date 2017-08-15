using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ScorchEngine.Server
{
    public static class ServerRoutes
    {
        public const string GetGamesApiUrl = "api/Games";
        public const string AddPlayerToGameApiUrl = "api/Games/{id}/Players";
        public const string GetGameApiUrl = "api/Games/{id}";

        public const string UpdatePlayerStateUrl = "api/Games/{id}/UpdateState";

        public const string ClearGamesUrl = "api/Games/Clear";

        public const string CreateGameUrl = "api/Games/Create";

        public const string SetPlayerInActiveUrl = "api/Games/{id}/RemovePlayer/{index}";

        public const string ServerBaseUri = "http://scorchar.azurewebsites.net";

        public const string LocalBaseUri = "http://localhost:17212/";
    }
}
