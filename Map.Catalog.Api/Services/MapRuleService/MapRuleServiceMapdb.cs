using Map.Catalog.Api.MapDB;

namespace Map.Catalog.Api.Services;

public class MapRuleServiceMapdb : IMapRuleService
{
    MapdbContext _mapdbContext;
    public MapRuleServiceMapdb(MapdbContext mapdbContext)
    {
        _mapdbContext = mapdbContext;
    }

    public IEnumerable<(string sourcePath, string destinationPath)> GetRule(string sourceName, string destinationName)
    {
        var rules = MapByMaster(GetRulesMapToMaster(sourceName), GetRulesMapFromMaster(destinationName)).ToArray();

        return rules;
    }
    private IEnumerable<(string sourcePath, string destinationPath)> MapByMaster(
        Dictionary<string, string> source,
        Dictionary<string, string> destination
    )
    {
        foreach (var item in source)
        {
            if (destination.TryGetValue(item.Value, out string? destinationPath))
                yield return (item.Key, destinationPath);
        }
    }

    private Dictionary<string, string> GetRulesMapToMaster(string sourceName)
    {
        var result = new Dictionary<string, string>();
        var rules = _mapdbContext.MapHeatingSystems
                .Where(s => s.SourceMap == sourceName && s.DestinationMap == "master");
        foreach (var rule in rules)
        {
            result.Add(rule.SourcePath, rule.DestinationPath);
        }

        return result;
    }
    private Dictionary<string, string> GetRulesMapFromMaster(string destinationName)
    {
        var result = new Dictionary<string, string>();
        var rules = _mapdbContext.MapHeatingSystems
                .Where(s => s.SourceMap == "master" && s.DestinationMap == destinationName);
        foreach (var rule in rules)
        {
            result.Add(rule.SourcePath, rule.DestinationPath);
        }

        return result;
    }
}
