using MemeTokenHub.Backoffce.Services.Interfaces;

namespace MemeTokenHub.Backoffce.Services
{
    public class LocalUploadService : IUploadService
    {
        public LocalUploadService()
        {
        }

        public async Task<bool> RemoveAsync(string memePageId, string section, string fileName)
        {
            return await Task.FromResult(true);
        }

        public async Task<string> UploadAsync(string memePageId, string section, string fileName, Stream stream)
        {
            var uploads = $"{CreatePath(memePageId)}\\uploads\\{section}";
            if (!Directory.Exists(uploads)) Directory.CreateDirectory(uploads);
            var filePath = Path.Combine(uploads, fileName);
            stream.CopyTo(new FileStream(filePath, FileMode.Create));

            return await Task.FromResult(filePath);
        }

        private static string CreatePath(string tenantId)
        {
            return $"mediaasset\\{tenantId}";
        }
    }
}
