using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ScorchServer.Db
{
    using MongoDB.Bson;
    using MongoDB.Driver;

    using ScorchServer.Models;

    public class UserRepository
    {
        private readonly IMongoDatabase r_Context = ScorchContext.Instance;

        public UserRepository()
        {
            Users = r_Context.GetCollection<User>(typeof(User).Name);
        }

        public IMongoCollection<User> Users { get; }

        public User Find(string i_Id)
        {
            return Users.Find(user => user.Id.Equals(i_Id)).First();
        }

        public void Insert(User i_User)
        {
            Users.InsertOne(i_User);
        }

        public void Delete(string i_Id)
        {
            Users.DeleteOne(user => user.Id.Equals(i_Id));
        }

        public void Update(User i_User)
        {
            Users.ReplaceOne(user => user.Id.Equals(i_User.Id),i_User);
        }

    }
}