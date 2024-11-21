using MemeTokenHub.Backoffce.Models;
using MemeTokenHub.Backoffce.Mongo.Interfaces;
using MemeTokenHub.Backoffce.Services.Interfaces;
using System;

namespace MemeTokenHub.Backoffce.Services
{
    public class MemePageService(IRepository<MemePageModel> repository) : BaseService<MemePageModel>(repository), IMemePageService
    {
        public async Task<IEnumerable<MemePageModel>> GetByOwnerIdAsync(string partnerId)
        {
            return await _repository.GetByFilter((x) => x.OwnerIds != null && x.OwnerIds.Contains(partnerId));
        }

        public override async Task CreateAsync(MemePageModel newModel)
        {
            if (newModel != null)
            {
                newModel.Status = PageStatus.Created;
                newModel.Id = GenerateId();
                newModel.PathUrl = GeneratePath(newModel.Name);

                if (string.IsNullOrEmpty(newModel.Id))
                {
                    throw new ArgumentNullException(nameof(newModel.Id));
                }

                await base.CreateAsync(newModel);
            }
        }

        private static string? GeneratePath(string? name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentNullException(nameof(name));
            }

            var path = name.Replace(" ", "").ToLowerInvariant();
            return path;
        }
    }
}