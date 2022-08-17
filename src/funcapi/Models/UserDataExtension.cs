using System.Text.Json.Serialization;

namespace Sg.Management.Models
{
    public class UserDataExtension
    {
        [JsonPropertyName("version")]
        public string Version { get; set; } = "1.0.0";

        [JsonPropertyName("action")]
        public string Action { get; set; } = "Continue";

        [JsonPropertyName("extension_tenant")]
        public string? Tenant { get; set; }

    }
}
