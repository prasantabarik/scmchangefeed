using System;
using System.Threading.Tasks;
using MongoDB.Driver;
using MongoDB.Bson;
using TCS.MVP.DeliveryMoment.ConfluentKafkaUtility;

namespace TCS.MVP.DeliveryMoment.DeliveryMomentCosmosRepository
{
   public class DeliveryMomentChangeStream
    {
        public static async Task StartChangeStream()
        {
            Console.WriteLine("StartChangeStream Method start");
            await TriggerChangeStreamAsync();
            Console.WriteLine("StartChangeStream Method End");
        }

        private static async Task TriggerChangeStreamAsync()
        {
            Console.WriteLine("TriggerChangeStream Method start");
            try
            {
                MongoClient dbClient = new MongoClient("mongodb://deliverymoment-cosmos-mongo-db:0LTdfL6WYtDKS4zINtXwkbUbt9UV4sZt0RA0KCqTgrn0qmXHWMQrgXmGQd7uYueYgClYYUaXjav8EzDCcty1xg==@deliverymoment-cosmos-mongo-db.mongo.cosmos.azure.com:10255/?ssl=true&replicaSet=globaldb&maxIdleTimeMS=120000&appName=@deliverymoment-cosmos-mongo-db@");
                //Database List  

                //Get Database and Collection  
                Console.WriteLine("Get Database : deliverymoment-db :");
               
                IMongoDatabase db = dbClient.GetDatabase("deliverymoment-db");

                Console.WriteLine("Get Deilverymoment Collection :");

                var deliveryColl = db.GetCollection<BsonDocument>("delivery-moment​​");

                Console.WriteLine("Changestream code start");

                //change feed code start
                var pipeline = new EmptyPipelineDefinition<ChangeStreamDocument<BsonDocument>>()
    .Match(change => change.OperationType == ChangeStreamOperationType.Insert || change.OperationType == ChangeStreamOperationType.Update || change.OperationType == ChangeStreamOperationType.Replace)
    .AppendStage<ChangeStreamDocument<BsonDocument>, ChangeStreamDocument<BsonDocument>, BsonDocument>(
    "{ $project: { '_id': 1, 'fullDocument': 1, 'ns': 1, 'documentKey': 1 }}");

                var options = new ChangeStreamOptions
                {
                    FullDocument = ChangeStreamFullDocumentOption.UpdateLookup
                };

                var enumerator = deliveryColl.Watch(pipeline, options).ToEnumerable().GetEnumerator();

                Console.WriteLine("Reading Change stream while loop start");
                while (enumerator.MoveNext())
                {
                    //publish message to Confluent Kafka
                    var document = enumerator.Current;
                    Console.WriteLine($"Start publishing messaage : {document}");
                    await DeliveryMomentMessageHandler.PublishMessage(document.ToString());
                    Console.WriteLine($"End publishing messaage : {document}");
                }

                enumerator.Dispose();

                Console.WriteLine("Reading Change stream while loop end");
                //change feed code end

            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }

            Console.WriteLine("TriggerChangeStream Method end");
        }
    }
}

