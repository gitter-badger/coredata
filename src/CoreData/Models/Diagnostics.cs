using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreData.Models
{
    public class Diagnostics
    {
        public string BrowserName { get; set; } = string.Empty;
        public string IpAddress { get; set; } = string.Empty;
        public string IsMobileBrowser { get; set; } = string.Empty;
    }
}
