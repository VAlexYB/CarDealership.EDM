using CarDealership.EDM.Core.Abstractions.Handlers;
using CarDealership.Shared.Enums;

namespace CarDealership.EDM.Core.Abstractions.External
{
    public interface IDataGrpcServiceClient
    {
        Task<BaseGenerator> GetData(string entity, DocumentType documentType, Guid entityId);
    }
}
