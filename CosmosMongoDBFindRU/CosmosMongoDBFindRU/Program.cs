using System;
using System.Linq;
using MongoDB.Bson;
using MongoDB.Driver;

namespace CosmosMongoDBFindRU
{
    class Program
    {
        static void Main(string[] args)
        {


            double totalRUCount = 0;
            Console.WriteLine("Hello World!");

            MongoClient dbClient = new MongoClient("mongodb://stage-omi-gpmd-mongocosmosdb:fJrJRDTsgPdDmMDu32ANTAsKt6M0NcZRzJ47m7P8Y98PFw46TYOFJ2Kq4JKD1lNkpUoPPtomlGHKVwP85PcFlA==@stage-omi-gpmd-mongocosmosdb.mongo.cosmos.azure.com:10255/?ssl=true&replicaSet=globaldb&retrywrites=false&maxIdleTimeMS=120000&appName=@stage-omi-gpmd-mongocosmosdb@");

            var database = dbClient.GetDatabase("HelplineDB");
            var collection = database.GetCollection<BsonDocument>("complaints");

            //var count = collection.Find(l => true).CountDocuments();

            // var count = collection.Find(l => l["constituencyNumber"] == 19).CountDocuments();

            // var uu = collection.Find(x => true).SortByDescending(bson => bson["_id"]).Limit(1).First();

            foreach (var i in Enumerable.Range(1,100))
            {
                var result = FindRuResult(collection, database,i, ref totalRUCount);

                Console.WriteLine("loop -" + i +": " + totalRUCount );
                Console.WriteLine( "RU: " + result.RequestCharge);
            }

            Console.WriteLine("Total : " + totalRUCount);
        }

        private static Root FindRuResult(IMongoCollection<BsonDocument> collection, IMongoDatabase database, int skip, ref double totalRUCount)
        {
            var uu = collection.EstimatedDocumentCount();

            var countResult = database.RunCommand<Root>(new BsonDocument {{"getLastRequestStatistics", 1}});


            totalRUCount = totalRUCount + countResult.RequestCharge;

            var yy = collection.Find(x => true).SortByDescending(bson => bson["_id"]).Skip(skip).Limit(10).ToList();

            var result = database.RunCommand<Root>(new BsonDocument {{"getLastRequestStatistics", 1}});

            totalRUCount = totalRUCount + result.RequestCharge;
            return result;
        }
    }

    public class Root
    {
        public string CommandName { get; set; }
        public double RequestCharge { get; set; }
        public int RequestDurationInMilliSeconds { get; set; }
        public int EstimatedDelayFromRateLimitingInMilliseconds { get; set; }
        public bool RetriedDueToRateLimiting { get; set; }
        public string ActivityId { get; set; }
        public double ok { get; set; }
    }
}
