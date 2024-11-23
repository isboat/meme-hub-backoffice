using Microsoft.Extensions.Options;
using MemeTokenHub.Backoffce.Mongo.Interfaces;
using Meme.Domain.Models;

namespace MemeTokenHub.Backoffce.Mongo
{


    public class MemePageRepository : BaseRepository<MemePageModel>, IRepository<MemePageModel>
    {
        public MemePageRepository(IOptions<MongoSettings> settings):base(settings, Constants.DatabaseName, Constants.MemePagesCollection)
        {
        }
    }
}