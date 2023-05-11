using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace emailappservice.Utility
{
    public class EmailMessage
    {
        public string Type { get; set; } = string.Empty;
        public string Receiver { get; set; } = string.Empty;
        public string Sender { get; set; } = string.Empty;
        public string Token { get; set; } = string.Empty;
        public string ProjectName { get; set; } = string.Empty;
        public string ProjectId { get; set; } = string.Empty;
    }
}
