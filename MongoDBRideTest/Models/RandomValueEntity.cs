using MongoDB.Bson.Serialization.Attributes;
using System;

namespace MongoDBRideTest.Models
{
    public class RandomValueEntity
    {
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string Id { get; set; }
        public string RandomValue { get; set; }
        public DateTime CreatedDate { get; set; }
        public override string ToString()
        {
            return $"Id: {Id}, Value: {RandomValue}, Created Date: {CreatedDate}";
        }
    }
}
