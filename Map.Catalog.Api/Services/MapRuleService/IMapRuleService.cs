namespace Map.Catalog.Api.Services;

public interface IMapRuleService
{
    IEnumerable<(string sourcePath, string destinationPath)> GetRule(string sourceName, string destinationName);
}
