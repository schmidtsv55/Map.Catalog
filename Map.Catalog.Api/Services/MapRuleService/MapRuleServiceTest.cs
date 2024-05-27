using Map.Catalog.Api.Services;

namespace Map.Catalog.Api.Services;

public class MapRuleServiceTest : IMapRuleService
{
    public IEnumerable<(string sourcePath, string destinationPath)> GetRule(string sourceName, string destinationName)
    {
        if (sourceName == "yandex" && destinationName == "wildberries")
        {
            return MapByMaster(YandexToMaster, MasterToWildberries);
        }
        if (sourceName == "wildberries" && destinationName == "yandex")
        {
            return MapByMaster(WildberriesToMaster, MasterToYandex);
        };
        throw new NotImplementedException($"Not implenent for sourceName {sourceName} destinationName {destinationName}");
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
    private static Dictionary<string, string> MasterToWildberries = new ()
    {
        {"name", "label"},
        {"madeby", "country.name"},
        {"color", "color"}
    };

    private static Dictionary<string, string> MasterToYandex = new()
    {
        {"name", "name"},
        {"madeby", "madebycountry" },
        {"color", "color.name" }
    };

    private static new Dictionary<string, string> WildberriesToMaster = new()
    {
        {"label", "name"},
        {"country.name", "madeby" },
        {"color", "color" }
    };
    private static new Dictionary<string, string> YandexToMaster = new()
    {
        {"name", "name"},
        {"madebycountry", "madeby" },
        {"color.name", "color" }
    };
}
