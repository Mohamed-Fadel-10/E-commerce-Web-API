using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace E_commerceAPI.Entities.Models
{
    public class Token
    {
        public string token { get; set; }

        public DateTime ExpireOn { get; set; }

        public string Message { get; set; } = string.Empty;
        [JsonIgnore]
        public bool IsAuthenticated { get; set; }

    }
}
