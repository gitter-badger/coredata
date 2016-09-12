using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreData.Models
{
    public class Article
    {
        public string Name { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public DateTime CreatedUtc { get; set; } = DateTime.UtcNow;
    }
}
