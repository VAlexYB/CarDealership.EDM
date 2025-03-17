using CarDealership.EDM.Core.Abstractions.External;
using CarDealership.EDM.Core.Abstractions.Services;
using CarDealership.EDM.Core.Repositories.Abstraction;
using CarDealership.EDM.DataAccess.Repositories;
using CarDealership.EDM.External.GrpcClients;
using CarDealership.EDM.Services;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;

var storageGrpcServerUrl = Environment.GetEnvironmentVariable("STORAGE_GRPC_SERVER_URL") ?? "localhost:7292";
services.AddGrpcClient<StorageGrpcServiceClient>(options =>
{
    options.Address = new Uri(storageGrpcServerUrl);
});
services.AddScoped<IStorageGrpcServiceClient, StorageGrpcServiceClient>();

var dataGrpcServerUrl = Environment.GetEnvironmentVariable("DATA_GRPC_SERVER_URL") ?? "localhost:7243";
services.AddGrpcClient<DataGrpcServiceClient>(options =>
{
    options.Address = new Uri(storageGrpcServerUrl);
});

services.AddControllers();

services.AddEndpointsApiExplorer();

services.AddScoped<IDataGrpcServiceClient, DataGrpcServiceClient>();

services.AddScoped<IDocumentsRepository, DocumentsRepository>();
services.AddScoped<ITemplatesRepository, TemplatesRepository>();

services.AddScoped<IDocumentsService, DocumentsService>();

var app = builder.Build();

app.MapControllers();

app.Run();
