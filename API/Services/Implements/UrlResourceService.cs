using API.Services.Interfaces;
using Domain.Models;
using Persistence.Repositories.Interfaces;
using System.Net.Mail;

namespace API.Services.Implements
{
    public class UrlResourceService : IUrlResourceService
    {
        private readonly IRepositoryBase<UrlResource> _repo;

        public UrlResourceService(IRepositoryBase<UrlResource> repo)
        {
            _repo = repo;
        }

        public async Task<List<UrlResource>> Get(string table, int entityId)
        {
            var result = await _repo.WhereAsync(x => x.Table.Equals(table) && x.EntityId == entityId);
            return result.ToList();
        }

        public async Task<List<UrlResource>> Add(string table, int entityId, List<string>? urls)
        {
            List<UrlResource> newList = new();
            if (urls == null) return newList;
            foreach (var url in urls)
            {
                UrlResource newUrl = new UrlResource()
                {   
                    Table = table,
                    EntityId = entityId,
                    Url = url
                };
                newList.Add(newUrl);
            }
            await _repo.CreateAsync(newList);
            return newList;
        }

        public async Task<List<UrlResource>> Update(string table, int entityId, List<string>? urls)
        {
            var target = await _repo.WhereAsync(x => x.Table.Equals(table) && x.EntityId == entityId);
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
                    Url = url
                };
                newList.Add(newUrl);
            }
            await _repo.CreateAsync(newList);
            return newList;
        }

        public async Task Delete(string table, int entityId)
        {
            var target = await _repo.WhereAsync(x => x.Table.Equals(table) && x.EntityId == entityId);
            foreach (var entity in target)
            {
                await _repo.DeleteAsync(entity);
            }
        }
    }
}
