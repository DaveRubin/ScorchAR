using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MongoDB.Bson.Serialization.Attributes;

namespace ScorchServer.Models
{
    public class User
    {
        [BsonId]
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Money { get; set; }
        public int NumberOfWins { get; set; }
        public int NumberOfKills { get; set; }
        public int NumberOfDeaths { get; set; }

        public User(string i_Id)
        {
            Id = i_Id;
        }

        protected bool Equals(User other)
        {
            return string.Equals(Id, other.Id) && string.Equals(FirstName, other.FirstName) && string.Equals(LastName, other.LastName) && Money == other.Money && NumberOfWins == other.NumberOfWins && NumberOfKills == other.NumberOfKills && NumberOfDeaths == other.NumberOfDeaths;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((User)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = (Id != null ? Id.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (FirstName != null ? FirstName.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (LastName != null ? LastName.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ Money;
                hashCode = (hashCode * 397) ^ NumberOfWins;
                hashCode = (hashCode * 397) ^ NumberOfKills;
                hashCode = (hashCode * 397) ^ NumberOfDeaths;
                return hashCode;
            }
        }

        public override string ToString()
        {
            return
$@"id: {Id}
FirstName: {FirstName}
LastName:  {LastName}
Money:  {Money}
NumberOfWins:{NumberOfWins}
NumberOfKills:{NumberOfKills}
NumberOfDeaths:{NumberOfDeaths}";

        }
    }
}