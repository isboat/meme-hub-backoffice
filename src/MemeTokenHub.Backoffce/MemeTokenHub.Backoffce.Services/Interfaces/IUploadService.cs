namespace MemeTokenHub.Backoffce.Services.Interfaces
{
    public interface IUploadService
    {
        Task<bool> RemoveAsync(string memePageId, string section, string filename);
        Task<string> UploadAsync(string memePageId, string section, string fileName, Stream stream);
    }
}
