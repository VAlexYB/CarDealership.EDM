using CarDealership.EDM.Core.Abstractions.External;
using Grpc.Core;
using Storage;
using static Storage.FileStorage;

namespace CarDealership.EDM.External.GrpcClients
{
    public class StorageGrpcServiceClient : IStorageGrpcServiceClient
    {
        private readonly FileStorageClient _grpcClient;
        private AsyncServerStreamingCall<DownloadResponse>? _downloadCall;
        private AsyncClientStreamingCall<UploadRequest, UploadResponse>? _uploadCall;

        public StorageGrpcServiceClient(FileStorageClient client)
        {
            _grpcClient = client;
        }

        public async IAsyncEnumerable<byte[]> DownloadFile(string directory, string filename)
        {
            if (_downloadCall == null)
            {
                _downloadCall = _grpcClient.DownloadFile(new DownloadRequest
                {
                    Directory = directory,
                    FileName = filename
                });
            }

            await foreach (var response in _downloadCall.ResponseStream.ReadAllAsync())
            {
                yield return response.Content.ToByteArray();
            }

        }

        public async Task UploadFile(string directory, string filename, byte[] content)
        {
            if (_uploadCall == null)
            {
                _uploadCall = _grpcClient.UploadFile();
            }

            var request = new UploadRequest
            {
                Directory = directory,
                FileName = filename,
                Content = Google.Protobuf.ByteString.CopyFrom(content)
            };

            await _uploadCall.RequestStream.WriteAsync(request);
        }

        public void Dispose()
        {
            _downloadCall?.Dispose();
            _uploadCall?.Dispose();
            _downloadCall = null;
            _uploadCall = null;
        }
    }
}
