using CarDealership.EDM.Core.Abstractions.External;
using CarDealership.EDM.Core.Abstractions.Handlers;
using CarDealership.EDM.Factories;
using CarDealership.Shared.Enums;
using Data;
using Grpc.Net.Client;
using static Data.DataService;

namespace CarDealership.EDM.External.GrpcClients
{
    public class DataGrpcServiceClient : IDataGrpcServiceClient
    {
        private readonly DataServiceClient _grpcClient;

        public DataGrpcServiceClient(DataServiceClient client)
        {
            _grpcClient = client;
        }

        public async Task<BaseGenerator> GetData(string entity, DocumentType documentType, Guid entityId)
        {
            var dataRequest = new DataRequest();
            dataRequest.EntityName = entity;
            dataRequest.DocType = (int)documentType;
            dataRequest.EntityId = entityId.ToString();

            var response = await _grpcClient.GetDataAsync(dataRequest);

            return GeneratorFactory.Create(response);
        }
    }
}
