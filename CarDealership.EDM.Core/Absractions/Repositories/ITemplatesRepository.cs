using CarDealership.EDM.Core.DTOs;
using CarDealership.EDM.Core.Models;
using CarDealership.Shared.Enums;
using MongoDB.Bson;

namespace CarDealership.EDM.Core.Repositories.Abstraction
{
    public interface ITemplatesRepository
    {
        Task<TemplateDTO> GetTemplate(string entity, DocumentType documentType, DocumentFormat documentFormat);
        Task<ObjectId> GetTemplateId(string entity, DocumentType documentType, DocumentFormat documentFormat);
    }
}
