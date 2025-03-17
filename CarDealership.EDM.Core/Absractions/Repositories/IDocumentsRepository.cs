using CarDealership.EDM.Core.DTOs;
using MongoDB.Bson;

namespace CarDealership.EDM.Core.Repositories.Abstraction
{
    public interface IDocumentsRepository
    {
        Task<DocumentDTO> GetDocumentPath(ObjectId templateId, Guid entityId);
        Task AddOrUpdateDocument(ObjectId templateId, Guid entityId, string directory, string filename);
    }
}
