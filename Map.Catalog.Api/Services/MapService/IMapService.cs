using System.Text.Json.Nodes;

namespace Map.Catalog.Api.Services;

public interface IMapService
{
    JsonObject Map(JsonObject source, string sourceName, string destinationName);
    JsonObject Map(object source, string sourceName, string destinationName);
}
