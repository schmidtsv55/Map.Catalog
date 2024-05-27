using System.Text.Json.Nodes;

namespace Map.Catalog.Api.Logic;

public interface IMapper
{
    JsonObject Convert(
        JsonObject source, 
        IEnumerable<(string sourcePath, string destinationPath)> rules);
    JsonNode? GetValue(JsonObject source, string sourcePath);
}
