using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class PropertyType : BaseEntity
    {
        public PropertyType()
        {
            Posts = new HashSet<Post>();
        }

        public string Name { get; set; }

        [JsonIgnore]
        public virtual ICollection<Post>? Posts { get; set; }

    }
}
