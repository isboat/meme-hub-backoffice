
using Meme.Domain.Models;

namespace MemeTokenHub.Backoffce.Services.Interfaces
{
    public interface IMemePageService : IService<MemePageModel>
    {
        Task<IEnumerable<MemePageModel>> GetByOwnerIdAsync(string ownerId);
    }
}
