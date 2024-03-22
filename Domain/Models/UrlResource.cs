using Domain.Constants.Enums;

namespace Domain.Models
{
    public class UrlResource : BaseEntity
    {      
        public string Table { get; set; }

        public int EntityId { get; set; }

        public string Url { get; set; }

        public ResourceType ResourceType { get; set; }

    }
}
