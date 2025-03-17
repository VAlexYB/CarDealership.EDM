using CarDealership.EDM.Core.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace CarDealership.EDM.DataAccess
{
    public class CDEdmDbContext
    {
        private IMongoDatabase _database;
        public CDEdmDbContext(IOptions<MongoConnectionOptions> options)
        {
            MongoClient client = new MongoClient(options.Value.ConnectionString);
            _database = client.GetDatabase(options.Value.DBName);
        }

        public IMongoCollection<Template> Templates =>
            _database.GetCollection<Template>(nameof(Template));

        public IMongoCollection<Document> Documents =>
            _database.GetCollection<Document>(nameof(Document));
    }
}
