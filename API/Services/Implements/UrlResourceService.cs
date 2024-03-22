using API.Services.Interfaces;
using Domain.Constants.Enums;
using Domain.Models;
using Persistence.Repositories.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Services.Implements
{
    public class UrlResourceService : IUrlResourceService
    {
        private readonly IRepositoryBase<UrlResource> _repo;

        public UrlResourceService(IRepositoryBase<UrlResource> repo)
        {
            _repo = repo;
        }

        public async Task<List<UrlResource>> Get(string table, int entityId, ResourceType resourceType)
        {
            var result = await _repo.WhereAsync(x => x.Table.Equals(table) && x.EntityId == entityId && (int)x.ResourceType == (int)resourceType);
            return result.ToList();
        }

        public async Task<List<string>> GetUrls(string table, int entityId, ResourceType resourceType)
        {
            var urlResources = await _repo.WhereAsync(x => x.Table.Equals(table) && x.EntityId == entityId && (int)x.ResourceType == (int)resourceType);
            return urlResources.Select(x => x.Url).ToList();
        }

        public async Task<List<UrlResource>> Add(string table, int entityId, List<string>? urls, ResourceType resourceType)
        {
            List<UrlResource> newList = new();
            if (urls == null) return newList;
            foreach (var url in urls)
            {
                UrlResource newUrl = new UrlResource()
                {
                    Table = table,
                    EntityId = entityId,
                    Url = url,
                    ResourceType = resourceType
                };
                newList.Add(newUrl);
            }
            await _repo.CreateAsync(newList);
            return newList;
        }

        public async Task<List<UrlResource>> Update(string table, int entityId, List<string>? urls, ResourceType resourceType)
        {
            var target = await _repo.WhereAsync(x => x.Table.Equals(table) && x.EntityId == entityId && (int)x.ResourceType == (int)resourceType);
            foreach (var entity in target)
            {
                await _repo.DeleteAsync(entity);
            }

            List<UrlResource> newList = new();
            if (urls == null) return newList;

            foreach (var url in urls)
            {
                UrlResource newUrl = new UrlResource()
                {
                    Table = table,
                    EntityId = entityId,
                    Url = url,
                    ResourceType = resourceType
                };
                newList.Add(newUrl);
            }
            await _repo.CreateAsync(newList);
            return newList;
        }

        public async Task Delete(string table, int entityId, ResourceType resourceType)
        {
            var target = await _repo.WhereAsync(x => x.Table.Equals(table) && x.EntityId == entityId && (int)x.ResourceType == (int)resourceType);
            foreach (var entity in target)
            {
                await _repo.DeleteAsync(entity);
            }
        }
    }
}
