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
        //private readonly IMongoDatabase r_Context = ScorchContext.Instance;
       // public IMongoCollection<User> Users { get; }

        public  Dictionary<string, User> Users { get; }

        public UserRepository()
        {
            Users =  new Dictionary<string, User>();// r_Context.GetCollection<User>(typeof(User).Name);
            User user = new User("AAAA") { FirstName = "Dushi", LastName = "Ben-Dushon" };
            Users.Add(user.Id,user);
        }

        public int GetNumberOfUsers()
        {
            return Users.Count;
        }


        public User Find(string i_Id)
        {
            //return Users.Find(user => user.Id.Equals(i_Id)).First();
            return Users[i_Id];
        }

        public void Insert(User i_User)
        {
            // Users.InsertOne(i_User);
            if (!Users.ContainsKey(i_User.Id))
            {
                Users.Add(i_User.Id,i_User);
            }
        }

        public void Delete(string i_Id)
        {
            //  Users.DeleteOne(user => user.Id.Equals(i_Id));
            if (Users.ContainsKey(i_Id))
            {
                Users.Remove(i_Id);
            }
        }

        public void Update(User i_User)
        {
           // Users.ReplaceOne(user => user.Id.Equals(i_User.Id),i_User);
            Users.Remove(i_User.Id);
            Users.Add(i_User.Id,i_User);
        }

    }
}