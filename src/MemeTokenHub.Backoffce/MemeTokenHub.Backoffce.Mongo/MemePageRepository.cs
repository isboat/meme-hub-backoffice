using Microsoft.Extensions.Options;
using System.Linq.Expressions;
using MemeTokenHub.Backoffce.Models;
using MemeTokenHub.Backoffce.Mongo.Interfaces;

namespace MemeTokenHub.Backoffce.Mongo
{


    public class MemePageRepository : BaseRepository<MemePageModel>, IRepository<MemePageModel>
    {
        public MemePageRepository(IOptions<MongoSettings> settings):base(settings, Constants.DatabaseName, Constants.MemePagesCollection)
        {
        }
    }
}