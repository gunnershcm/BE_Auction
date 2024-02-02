using API.DTOs.Requests.Properties;
using API.DTOs.Responses.Properties;
using Domain.Models;

namespace API.Services.Interfaces
{
    public interface IPropertyService
    {
        Task<List<GetPropertyResponse>> Get();
        Task<Property> CreateProperty(int postId, CreatePropertyRequest model);
    }
}
