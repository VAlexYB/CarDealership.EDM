using MongoDB.Bson;

namespace CarDealership.EDM.Core.DTOs
{
    public record TemplateDTO
    (
        ObjectId TemplateId, 
        string Directory, 
        string Filename
    );
}
