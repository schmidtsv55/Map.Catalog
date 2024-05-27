using System.Text.Json.Nodes;

namespace Map.Catalog.Api.Logic;

public class Mapper : IMapper
{

    private string[]? GetPath(JsonObject source, string destinationPath)
    {
        var keys = destinationPath.Split(".");
        if (keys.Length == 1)
            return [destinationPath];


        for (int i = 0; i < keys.Length; i++)
        {
            if (source.ContainsKey(keys[i]))
                continue;

            var result = new string[keys.Length - i];
            Array.Copy(keys, i, result, 0, result.Length);
            return result;

        }

        return null;
    }

    private (string key, JsonNode node) ConvertToNode(JsonNode value, string[] path)
    {
        var key = path[0];
        if (path.Length == 1)
            return (key, value);

        var innerNode = new JsonObject();
        for (int i = 1; i < path.Length; i++)
        {
            var nextKey = path[i];
            if (i == path.Length - 1)
            {
                innerNode.Add(nextKey, value);
                break;
            }
            var newInnerNode = new JsonObject();
            innerNode.Add(nextKey, newInnerNode);
            innerNode = newInnerNode;

        }

        return (key, innerNode);
    }
    private JsonObject GetLastParentPath(
        string path,
        JsonObject jo
    )
    {
        var parent = jo;
        var keys = path.Split('.');
        foreach (var key in keys)
        {
            if (!parent.ContainsKey(key))
                break;

            if (!parent.TryGetPropertyValue(
                key, out var node) || node == null)
            {
                throw new InvalidCastException($"invalid object in path {path}");
            }


            parent = node.AsObject();
        }

        return parent;
    }
    public JsonObject Convert(JsonObject source, IEnumerable<(string sourcePath, string destinationPath)> rules)
    {
        var result = new JsonObject();
        foreach (var rule in rules)
        {
            var value = GetValue(source, rule.sourcePath);
            if (value == null)
                continue;

            var path = GetPath(result, rule.destinationPath);
            if (path == null || path.Length == 0)
                continue;

            var cloneValue = JsonValue.Create(value.GetValue<object>());

            var keyValue = ConvertToNode(cloneValue!, path);
      
            var parent = GetLastParentPath(rule.destinationPath, result);
            parent.Add(keyValue.key, keyValue.node);

        }
        return result;
    }
    private JsonNode? GetValue(JsonObject obj, string[] keys)
    {
        if (obj == null)
            return null;

        var key = keys[0];
        if (!obj.TryGetPropertyValue(key, out var value))
            return null;

        if (value == null)
            return null;

        if (keys.Length == 1)
            return value;

        var newKeys = new string[keys.Length - 1];
        Array.Copy(keys, 1, newKeys, 0, newKeys.Length);
        return GetValue(value.AsObject(), newKeys);
    }
    public JsonNode? GetValue(JsonObject source, string sourcePath)
    {
        var keys = sourcePath.Split(".");
        if (keys.Length == 0)
            return null;

        return GetValue(source, keys);

    }
}
