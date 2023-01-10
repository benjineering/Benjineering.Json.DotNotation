using System.Text.Json;

namespace Benjineering.Json.DotNotation;

public static class JsonElementExtensions
{
    public static bool IsNullOrUndefined(this JsonElement jsonElement)
    {
        return JsonElementHelpers.IsNullOrUndefined(jsonElement);
    }

    public static JsonElement GetPropertyAtPath(this JsonElement jsonElement, string path)
    {
        return JsonElementHelpers.GetPropertyAtPath(jsonElement, path);
    }
}
