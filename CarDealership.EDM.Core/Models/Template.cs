using CarDealership.Shared.Enums;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace CarDealership.EDM.Core.Models
{
    public class Template
    {
        [BsonId]
        public ObjectId Id { get; set; }
        public string Entity {  get; set; }
        public DocumentType DocumentType { get; set; }
        public DocumentFormat DocumentFormat { get; set; }
        public string Directory { get; set; }
        public string Filename { get; set; }
    }
}
