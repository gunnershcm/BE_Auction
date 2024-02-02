using API.DTOs.Requests.Properties;
using API.DTOs.Responses.Properties;
using Domain.Models;

namespace API.Services.Interfaces
{
    public interface IPropertyService
    {
        Task<List<GetPropertyResponse>> Get();
        Task<GetPropertyResponse> GetById(int id);
        Task<Property> CreateProperty(int postId, CreatePropertyRequest model);
        Task<Property> UpdateProperty(int id, UpdatePropertyRequest model);
        Task Remove(int id);
    }
}
