using MongoDB.Bson;

namespace CarDealership.EDM.Core.DTOs
{
    public record DocumentDTO
    (
        ObjectId DocumentId,
        string Directory,
        string Filename
    );
}
