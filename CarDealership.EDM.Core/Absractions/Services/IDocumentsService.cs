using CarDealership.EDM.Core.DTOs;
using CarDealership.EDM.Core.Models;
using CarDealership.Shared.Enums;

namespace CarDealership.EDM.Core.Abstractions.Services
{
    public interface IDocumentsService
    {
        Task<Stream> GetDocument(string entity, DocumentType documentType, DocumentFormat documentFormat, Guid entityId);
        Task<string> CreateDocument(string entity, DocumentType documentType, DocumentFormat documentFormat, Guid entityId);
    }
}
