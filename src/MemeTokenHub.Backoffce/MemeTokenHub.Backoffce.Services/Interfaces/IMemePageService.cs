using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MemeTokenHub.Backoffce.Models;
using MemeTokenHub.Backoffce.Services;

namespace MemeTokenHub.Backoffce.Services.Interfaces
{
    public interface IMemePageService : IService<MemePageModel>
    {
        Task<IEnumerable<MemePageModel>> GetByOwnerIdAsync(string ownerId);
    }
}
