using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MemeTokenHub.Backoffce.Models;

namespace MemeTokenHub.Backoffce.Mongo.Interfaces
{
    public interface ITenantDBRepository<T> : IRepository<T>
    {
        public void CreateDB(string dbName);
    }
}
