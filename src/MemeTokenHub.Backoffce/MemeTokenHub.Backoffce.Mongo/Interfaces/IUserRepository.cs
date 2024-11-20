using MemeTokenHub.Backoffce.Models;

namespace MemeTokenHub.Backoffce.Mongo.Interfaces
{
    public interface IUserRepository : IRepository<UserModel>
    {
        public Task<List<UserModel>> GetUsersAsync(string tenantId);

        public Task<UserModel?> GetByEmailAsync(string email);
    }
}
