using MemeTokenHub.Backoffce.Models;
using MemeTokenHub.Backoffce.Mongo.Interfaces;
using MemeTokenHub.Backoffce.Services.Interfaces;

namespace MemeTokenHub.Backoffce.Services
{
    public class UserService : BaseService<UserModel>, IUserService
    {
        private readonly IEncryptionService _encryptionService;

        public UserService(IRepository<UserModel> repository, IEncryptionService encryptionService):base(repository)
        {
            _encryptionService = encryptionService;
        }

        public override async Task CreateAsync(UserModel newModel)
        {
            if (newModel != null)
            {
                newModel.Id = GenerateId();

                if (string.IsNullOrEmpty(newModel.Id))
                {
                    throw new ArgumentNullException(nameof(newModel.Id));
                }

                newModel.Password = _encryptionService.Encrypt(newModel?.Password!).Hashed;
                await _repository.CreateAsync(newModel);
            }
        }

        public async Task<UserModel?> LoginAsync(string email, string password)
        {
            var foundUsers = await _repository.GetByFilter(x => x.Email?.ToLowerInvariant() == email.ToLowerInvariant());
            if (foundUsers == null || !foundUsers.Any()) return null;
            
            if(foundUsers.Count() > 1)
            {
                throw new ArgumentException("More than one found");
            }

            var user = foundUsers.First();

            var passwdVerified = _encryptionService.Verify(password, user?.Password!);
            if (!passwdVerified) throw new ArgumentException("");

            return user;
        }
    }
}