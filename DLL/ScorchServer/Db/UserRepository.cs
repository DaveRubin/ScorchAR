using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ScorchServer.Db
{
    using MongoDB.Bson;
    using MongoDB.Driver;

    using ScorchEngine.Models;

    using ScorchServer.Models;

    public class UserRepository
    {
       private readonly IMongoDatabase r_Context = ScorchContext.Instance;
       public IMongoCollection<PlayerInfo> Users { get; }

       // public  Dictionary<string, User> Users { get; }

        public UserRepository()
        {
            Users = r_Context.GetCollection<PlayerInfo>(typeof(PlayerInfo).Name);

        }

        public long GetNumberOfUsers()
        {
            return Users.Count(new FilterDefinitionBuilder<PlayerInfo>().Empty);
        }


        public PlayerInfo Find(string Id)
        {
            return Users.Find(user => user.Id.Equals(Id)).First();
        }

        public void Insert(PlayerInfo playerInfo)
        {
             Users.InsertOne(playerInfo);
        }

        public void Delete(string Id)
        {
            Users.DeleteOne(user => user.Id.Equals(Id));
        }

        public void Update(PlayerInfo playerInfo)
        {
           Users.ReplaceOne(user => user.Id.Equals(playerInfo.Id), playerInfo);
        }

    }
}