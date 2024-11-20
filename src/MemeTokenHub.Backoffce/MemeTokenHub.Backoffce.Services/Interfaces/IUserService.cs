using MemeTokenHub.Backoffce.Models;

namespace MemeTokenHub.Backoffce.Services.Interfaces
{
    public interface IUserService: IService<UserModel>
    {
        Task<UserModel?> LoginAsync(string email, string password);
    }
}
