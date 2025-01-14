using System.Text.Json.Serialization;

namespace ServerCore.Models.App;

public class AppAPI
{
    [LiteDB.BsonId]
    [JsonPropertyName("applicationId")]
    public Guid ApplicationId { get; set; }

    [JsonPropertyName("description")]
    public string Description { get; set; } = string.Empty;

    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    [JsonPropertyName("displayName")]
    public string DisplayName { get; set; } = string.Empty;

    [JsonPropertyName("platform")]
    public string Platform { get; set; } = string.Empty;

    [JsonPropertyName("genre")]
    public string Genre { get; set; } = string.Empty;

    [JsonPropertyName("releaseDate")]
    public string ReleaseDate { get; set; } = string.Empty;

    [JsonPropertyName("spaceId")]
    public Guid SpaceId { get; set; }
}
