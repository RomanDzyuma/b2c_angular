using System.Text.Json.Serialization;

namespace Sg.Management.Models
{
    public record UserData(
        [property: JsonPropertyName("objectId")] string ObjectId);
}