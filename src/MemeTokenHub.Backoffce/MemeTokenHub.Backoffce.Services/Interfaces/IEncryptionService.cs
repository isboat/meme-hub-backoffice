using MemeTokenHub.Backoffce.Models;

namespace MemeTokenHub.Backoffce.Services.Interfaces
{
    public interface IEncryptionService
    {
        EncryptedResult? Encrypt(string input);
        bool Verify(string input, string storedHash);
    }
}
