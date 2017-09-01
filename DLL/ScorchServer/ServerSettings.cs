using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
//TODO Read Save From File
namespace ScorchServer
{
    public static class ServerSettings
    {
        public const string k_DbName = "scorchar";

        public const string ConnectionString =
            "mongodb://scorchar:S4W5F9T6y6oNwYQgont6W3z5t39n0AlucPmFpuca3vQrlKzQ1vSojosDfdtG3n14WSKrg2YPKqD92KCu6y3jiw==@scorchar.documents.azure.com:10255/?ssl=true&replicaSet=globaldb";
    }
}