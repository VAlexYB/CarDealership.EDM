namespace CarDealership.EDM.Core.Abstractions.External
{
    public interface IStorageGrpcServiceClient : IDisposable
    {
        IAsyncEnumerable<byte[]> DownloadFile(string directory, string filename);
        Task UploadFile(string directory, string filename, byte[] content);
    }
}
