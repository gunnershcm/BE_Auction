using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class PropertyType 
    {
        public PropertyType()
        {
            Properties = new HashSet<Property>();
        }

        public int Id { get; set; }

        public string Name { get; set; }

        [JsonIgnore]
        public virtual ICollection<Property>? Properties { get; set; }

    }
}
