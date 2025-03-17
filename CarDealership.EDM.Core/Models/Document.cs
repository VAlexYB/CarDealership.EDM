using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace CarDealership.EDM.Core.Models
{
    public class Document
    {
        [BsonId]
        public ObjectId Id { get; set; }
        public ObjectId TemplateId { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime EditDate { get; set; }
        public Guid EntityId { get; set; }
        public string Directory { get; set; }
        public string Filename { get; set; }
    }
}
