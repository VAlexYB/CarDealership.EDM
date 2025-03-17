using CarDealership.EDM.Core.DTOs;
using CarDealership.EDM.Core.Models;
using CarDealership.EDM.Core.Repositories.Abstraction;
using MongoDB.Bson;
using MongoDB.Driver;

namespace CarDealership.EDM.DataAccess.Repositories
{
    public class DocumentsRepository : IDocumentsRepository
    {
        private readonly CDEdmDbContext _context;
        public DocumentsRepository(CDEdmDbContext context)
        {
            _context = context;
        }

        public async Task AddOrUpdateDocument(ObjectId templateId, Guid entityId, string directory, string filename)
        {
            var document = await _context.Documents
                .Find(d => d.TemplateId == templateId && d.EntityId == entityId)
                .FirstOrDefaultAsync();

            if (document == null)
            {
                document = new Document
                {
                    TemplateId = templateId,
                    EntityId = entityId,
                    Directory = directory,
                    Filename = filename,
                    CreateDate = DateTime.UtcNow,
                    EditDate = DateTime.UtcNow
                };
                await _context.Documents.InsertOneAsync(document);
            }
            else
            {
                document.Directory = directory;
                document.Filename = filename;
                document.EditDate = DateTime.UtcNow;
                await _context.Documents.ReplaceOneAsync(d => d.TemplateId == templateId && d.EntityId == entityId, document);
            }
        }

        public async Task<DocumentDTO> GetDocumentPath(ObjectId templateId, Guid entityId)
        {
            return await _context.Documents
                .Find(d => d.TemplateId == templateId && d.EntityId == entityId)
                .Project(t => new DocumentDTO(t.Id, t.Directory, t.Filename))
                .FirstOrDefaultAsync();
        }
    }
}
