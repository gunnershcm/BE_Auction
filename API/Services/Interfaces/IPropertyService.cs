using API.DTOs.Responses.Properties;

namespace API.Services.Interfaces
{
    public interface IPropertyService
    {
        Task<List<GetPropertyResponse>> Get();
    }
}
