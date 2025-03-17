namespace CarDealership.EDM.Endpoints.Contracts.Requests
{
    public record CreateDocumentRequest
    {
        public string Entity {  get; init; }
        public Guid EntityId { get; init; }
        public int DocumentType { get; init; }
        public int DocumentFormat { get; init; }

        public CreateDocumentRequest(string entity, Guid entityId, int documentType, int documentFormat)
        {
            Entity = entity;
            EntityId = entityId;
            DocumentType = documentType;
            DocumentFormat = documentFormat;
        }

    }
}
