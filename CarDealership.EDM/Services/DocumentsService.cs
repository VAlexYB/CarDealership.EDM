using CarDealership.EDM.Core.Abstractions.External;
using CarDealership.EDM.Core.Abstractions.Handlers;
using CarDealership.EDM.Core.Abstractions.Services;
using CarDealership.EDM.Core.DTOs;
using CarDealership.EDM.Core.Models;
using CarDealership.EDM.Core.Repositories.Abstraction;
using CarDealership.Shared.Enums;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using System.IO;

namespace CarDealership.EDM.Services
{
    public class DocumentsService : IDocumentsService
    {
        private readonly IDataGrpcServiceClient _dataServiceClient;

        private readonly ITemplatesRepository _templatesRepository;
        private readonly IDocumentsRepository _documentsRepository;

        private readonly IServiceScopeFactory _serviceScopeFactory;

        public DocumentsService
        (
            IDataGrpcServiceClient dataServiceClient,
            ITemplatesRepository templatesRepository,
            IDocumentsRepository documentsRepository,
            IServiceScopeFactory serviceScopeFactory
        )
        {
            _dataServiceClient = dataServiceClient;
            _templatesRepository = templatesRepository;
            _documentsRepository = documentsRepository;
            _serviceScopeFactory = serviceScopeFactory;
        }

        public async Task<string> CreateDocument(string entity, DocumentType documentType, DocumentFormat documentFormat, Guid entityId)
        {
            BaseGenerator generator = await _dataServiceClient.GetData(entity, documentType, entityId);
            generator.DocumentFormat = documentFormat;

            var template = await _templatesRepository.GetTemplate(entity, documentType, documentFormat);

            if (template == null) throw new InvalidOperationException("Шаблона для указанного документа не существует");

            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var storageClient = scope.ServiceProvider.GetRequiredService<IStorageGrpcServiceClient>();

                await foreach (byte[] chank in storageClient.DownloadFile(template.Directory, template.Filename))
                {
                    byte[] processedContent = generator.PrepareDocument(chank);
                    await storageClient.UploadFile(generator.Directory, generator.FileName, processedContent);
                }

                byte[] finalContent = generator.Finalize();
                if (finalContent.Length > 0)
                {
                    await storageClient.UploadFile(generator.Directory, generator.FileName, finalContent);
                }
            }

            await _documentsRepository.AddOrUpdateDocument(template.TemplateId, entityId, generator.Directory, generator.FileName);

            return $"{generator.Directory}/{generator.FileName}";
        }

        public async Task<Stream> GetDocument(string entity, DocumentType documentType, DocumentFormat documentFormat, Guid entityId)
        {
            var templateId = await _templatesRepository.GetTemplateId(entity, documentType, documentFormat);
            var document = await _documentsRepository.GetDocumentPath(templateId, entityId);

            var stream = new MemoryStream();
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var storageClient = scope.ServiceProvider.GetRequiredService<IStorageGrpcServiceClient>();

                await foreach (byte[] chunk in storageClient.DownloadFile(document.Directory, document.Filename))
                {
                    await stream.WriteAsync(chunk, 0, chunk.Length);

                }
            }

            stream.Position = 0;

            return stream;
        }
    }
}
