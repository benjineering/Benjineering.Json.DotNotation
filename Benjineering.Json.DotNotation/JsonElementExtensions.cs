using System.Text.Json;

namespace Benjineering.Json.DotNotation;

public static class JsonElementExtensions
{
    public static bool IsNullOrUndefined(this JsonElement jsonElement)
    {
        return JsonElementHelpers.IsNullOrUndefined(jsonElement);
    }

    /// <summary>
    /// Throws an exception if the jsonElement is null or undefined
    /// </summary>
    /// <exception cref="ArgumentNullException"></exception>
    public static void EnsureNotNullOrUndefined(this JsonElement jsonElement)
    {
        if (jsonElement.ValueKind is JsonValueKind.Null or JsonValueKind.Undefined)
            throw new ArgumentNullException(nameof(jsonElement));
    }

    /// <summary>
    ///     Allows querying a JsonElement by path using dot notation e.g. user?.email<br /><br />
    ///     To query array items, use the index in place of a property name (if the index is out of range, 
    ///     an undefined element will be returned) e.g. country.states.0
    /// </summary>
    /// <exception cref="KeyNotFoundException"></exception>
    public static JsonElement GetPropertyAtPath(this JsonElement jsonElement, string path)
    {
        return JsonElementHelpers.GetPropertyAtPath(jsonElement, path);
    }
}
