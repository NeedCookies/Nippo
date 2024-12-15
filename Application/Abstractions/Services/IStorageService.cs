namespace Application.Abstractions.Services
{
    public interface IStorageService
    {
        Task PutAsync(string name, Stream mediaStream, string contentType);
        Task<string> GetUrlAsync(string objectName);
    }
}
