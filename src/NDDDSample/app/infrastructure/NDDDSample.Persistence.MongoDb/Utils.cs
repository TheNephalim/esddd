using MongoDB.Driver;

namespace NDDDSample.Persistence.MongoDb
{
    public class Utils
    {
        public static MongoDatabase ShippingDb
        {
            get
            {
                var client = new MongoClient();
                var server = client.GetServer();
                var db = server.GetDatabase("shipping");
                return db;
            }
           
        } 
    }
}