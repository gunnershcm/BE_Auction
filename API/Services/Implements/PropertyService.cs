using API.DTOs.Responses.Posts;
using API.DTOs.Responses.Properties;
using API.Services.Interfaces;
using AutoMapper;
using Domain.Models;
using Persistence.Repositories.Interfaces;

namespace API.Services.Implements
{
    public class PropertyService : IPropertyService
    {
        private readonly IRepositoryBase<Property> _propertyRepository;
        private readonly IMapper _mapper;

        public PropertyService(IRepositoryBase<Property> propertyRepository, IMapper mapper)
        {
            _propertyRepository = propertyRepository;
            _mapper = mapper;
        }

        public async Task<List<GetPropertyResponse>> Get()
        {
            var result = await _propertyRepository.GetAsync(navigationProperties: new string[]
                { "Author", "Post"});
            var response = _mapper.Map<List<GetPropertyResponse>>(result);
            return response;
        }

    }
}
