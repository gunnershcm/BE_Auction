using Domain.Models;

namespace API.Services.Interfaces
{
    public interface IUrlResourceService
    {
        Task<List<UrlResource>> Get(string table, int entityId);
        Task<List<UrlResource>> Add(string table, int entityId, List<string>? urls);
        Task<List<UrlResource>> Update(string table, int entityId, List<string>? urls);
        Task Delete(string table, int entityId);
    }
}
