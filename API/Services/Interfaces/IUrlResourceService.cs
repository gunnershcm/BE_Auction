using Domain.Constants.Enums;
using Domain.Models;

namespace API.Services.Interfaces
{
    public interface IUrlResourceService
    {
        Task<List<UrlResource>> Get(string table, int entityId, ResourceType resourceType);
        Task<List<string>> GetUrls(string table, int entityId, ResourceType resourceType);
        Task<List<UrlResource>> Add(string table, int entityId, List<string>? urls, ResourceType resourceType);
        Task<List<UrlResource>> Update(string table, int entityId, List<string>? urls, ResourceType resourceType);
        Task Delete(string table, int entityId, ResourceType resourceType);
    }
}
