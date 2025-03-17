using CarDealership.EDM.Core.DTOs;
using CarDealership.EDM.Core.Models;
using CarDealership.EDM.Core.Repositories.Abstraction;
using CarDealership.Shared.Enums;
using MongoDB.Bson;
using MongoDB.Driver;

namespace CarDealership.EDM.DataAccess.Repositories
{
    public class TemplatesRepository : ITemplatesRepository
    {
        private readonly CDEdmDbContext _context;

        public TemplatesRepository(CDEdmDbContext context)
        {
            _context = context;
        }

        public async Task<ObjectId> GetTemplateId(string entity, DocumentType documentType, DocumentFormat documentFormat)
        {
            return await _context.Templates
                .Find
                (
                    t => t.Entity == entity
                    && t.DocumentType == documentType
                    && t.DocumentFormat == documentFormat
                )
                .Project(t => t.Id)
                .FirstOrDefaultAsync();
        }

        public async Task<TemplateDTO> GetTemplate(string entity, DocumentType documentType, DocumentFormat documentFormat)
        {
            var filter = Builders<Template>.Filter.Eq(t => t.Entity, entity)
                & Builders<Template>.Filter.Eq(t => t.DocumentType, documentType)
                & Builders<Template>.Filter.Eq(t => t.DocumentFormat, documentFormat);

            return await _context.Templates
                .Find(filter)
                .Project(t => new TemplateDTO(t.Id, t.Directory, t.Filename))
                .FirstOrDefaultAsync();
        }
    }
}
