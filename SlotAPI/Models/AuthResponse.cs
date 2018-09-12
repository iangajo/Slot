using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace SlotAPI.Models
{
    public class AuthResponse
    {
        [JsonProperty(PropertyName = "token_type")]
        public string Type { get; } = "Bearer";

        [JsonProperty(PropertyName = "access_token")]
        public string Token { get; set; }

        [JsonProperty(PropertyName = "expires_in")]
        public int Expires { get; } = 1800;
    }
}
