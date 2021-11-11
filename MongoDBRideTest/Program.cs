using MongoDB.Driver;
using MongoDBRideTest.Models;
using System;
using System.Collections.Generic;

namespace MongoDBRideTest
{
    class Program
    {
        private static string _connectionString =
  @"mongodb://hero-ride-staging:YaJttRXNbPqkYbodXs2s1n2TXAwNmJnCUPjzc5dy3f8doDKi3vllbeelSzUys14k5CZkTCRChvYViDqnnPuFNw==@hero-ride-staging.mongo.cosmos.azure.com:10255/?ssl=true&retrywrites=false&replicaSet=globaldb&maxIdleTimeMS=120000&appName=@hero-ride-staging@";

        private static MongoClient _mongoClient;
        static Program()
        {
            var mongoSettings = MongoClientSettings.FromUrl(new MongoUrl(_connectionString));
            mongoSettings.SslSettings = new SslSettings
            {
                EnabledSslProtocols = System.Security.Authentication.SslProtocols.Tls12
            };
            _mongoClient = new MongoClient(mongoSettings);

        }
        static IMongoCollection<RandomValueEntity> GetCollection()
        {
            var db = _mongoClient.GetDatabase("RideDB");
            var collection = db.GetCollection<RandomValueEntity>("RandomValue");
            if(collection == null)
            {
                db.CreateCollection("RandomValue");
                collection = db.GetCollection<RandomValueEntity>("RandomValue");
            }
            return collection;
        }
        static void Create(List<RandomValueEntity> randomValueEntities)
        {
            var collection = GetCollection();
            collection.InsertMany(randomValueEntities);
            
        }
        static void FindAll()
        {
            var collection = GetCollection();
            var list = collection.Find(filter => true).ToList();
            foreach(var item in list)
            {
                Console.WriteLine(item.ToString());
            }
        }
        static void AddIndex()
        {
            var collection = GetCollection();
            var createIndexKeyDef = Builders<RandomValueEntity>.IndexKeys.Ascending(c => c.RandomValue);
            collection.Indexes.CreateOne(new CreateIndexModel<RandomValueEntity>(createIndexKeyDef));
            Console.WriteLine("Created");
        }
        static void Test()
        {
            Create(new List<RandomValueEntity>
            {
                new RandomValueEntity
                {
                    RandomValue = Guid.NewGuid().ToString().Replace("-",""),
                    CreatedDate = DateTime.UtcNow
                },
                new RandomValueEntity
                {
                    RandomValue = Guid.NewGuid().ToString().Replace("-",""),
                    CreatedDate = DateTime.UtcNow
                },
                new RandomValueEntity
                {
                    RandomValue = Guid.NewGuid().ToString().Replace("-",""),
                    CreatedDate = DateTime.UtcNow
                }
            });
            FindAll();
        }
        static void Main(string[] args)
        {
            //Test();
            AddIndex();
            Console.ReadLine();
        }
    }
}
