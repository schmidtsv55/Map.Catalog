using System.Text.Json;
using System.Text.Json.Nodes;
using Map.Catalog.Api.Logic;

namespace Map.Catalog.Api.Services;

public class MapService : IMapService
{
    IMapper _mapper;
    IMapRuleService _mapRuleService;
    public MapService(
        IMapper mapper,
        IMapRuleService mapRuleService)
    {
        _mapper = mapper;
        _mapRuleService = mapRuleService;
    }
    public JsonObject Map(JsonObject source, string sourceName, string destinationName)
    {
        var rule = _mapRuleService.GetRule(sourceName, destinationName);
        return _mapper.Convert(source, rule);
    }

    public JsonObject Map(object source, string sourceName, string destinationName)
    {

        var value = JsonValue.Create(source, new JsonNodeOptions
        {

        });
        var jo = value.Deserialize<JsonObject>();
        return Map(jo, sourceName, destinationName);
    }
}
