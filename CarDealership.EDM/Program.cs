using CarDealership.EDM.Core.Abstractions.External;
using CarDealership.EDM.Core.Abstractions.Services;
using CarDealership.EDM.Core.Repositories.Abstraction;
using CarDealership.EDM.DataAccess;
using CarDealership.EDM.DataAccess.Repositories;
using CarDealership.EDM.External.GrpcClients;
using CarDealership.EDM.Services;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(5220, listenOptions =>
    {
        listenOptions.Protocols = Microsoft.AspNetCore.Server.Kestrel.Core.HttpProtocols.Http1;
    });

    options.ListenAnyIP(7013, listenOptions =>
    {
        listenOptions.Protocols = Microsoft.AspNetCore.Server.Kestrel.Core.HttpProtocols.Http2;
    });
});

var services = builder.Services;

BsonSerializer.RegisterSerializer(new GuidSerializer(GuidRepresentation.Standard));

services.Configure<MongoConnectionOptions>(builder.Configuration.GetSection(nameof(MongoConnectionOptions)));
services.AddSingleton<CDEdmDbContext>();

var storageGrpcServerUrl = Environment.GetEnvironmentVariable("STORAGE_GRPC_SERVER_URL") ?? "http://localhost:7292";
services.AddGrpcClient<Storage.FileStorage.FileStorageClient>(options =>
{
    options.Address = new Uri(storageGrpcServerUrl);
});
services.AddScoped<IStorageGrpcServiceClient, StorageGrpcServiceClient>();

var dataGrpcServerUrl = Environment.GetEnvironmentVariable("DATA_GRPC_SERVER_URL") ?? "http://localhost:7243";
services.AddGrpcClient<Data.DataService.DataServiceClient>(options =>
{
    options.Address = new Uri(dataGrpcServerUrl);
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
