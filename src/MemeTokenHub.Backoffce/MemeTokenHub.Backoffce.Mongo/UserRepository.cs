using Microsoft.Extensions.Options;
using MemeTokenHub.Backoffce.Models;
using MemeTokenHub.Backoffce.Mongo.Interfaces;

namespace MemeTokenHub.Backoffce.Mongo
{
    public class UserRepository : BaseRepository<UserModel>, IRepository<UserModel>
    {
        public UserRepository(IOptions<MongoSettings> settings) : base(settings, Constants.DatabaseName, Constants.UserCollection)
        {
        }
    }
}