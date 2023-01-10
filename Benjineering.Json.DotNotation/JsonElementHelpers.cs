using System.Text.Json;

namespace Benjineering.Json.DotNotation;

public static class JsonElementHelpers
{
    private const char Dot = '.';
    private const char QuestionMark = '?';

    public static bool IsNullOrUndefined(JsonElement jsonElement)
    {
        return jsonElement.ValueKind is JsonValueKind.Null or JsonValueKind.Undefined;
    }

    /// <summary>
    /// Throws an exception if the jsonElement is null or undefined
    /// </summary>
    /// <exception cref="ArgumentNullException"></exception>
    public static void EnsureNotNullOrUndefined(JsonElement jsonElement)
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
    public static JsonElement GetPropertyAtPath(JsonElement jsonElement, string path)
    {
        if (path == null)
            throw new ArgumentNullException(nameof(path));

        if (path.Length == 0)
            throw new ArgumentException("Cannot be empty", path);

        int offset = 0;
        bool propertyIsNullable = false;

        PathSection pathSection;
        string propertyName;

        do
        {
            pathSection = GetPathSection(ref path, offset);
            propertyName = pathSection.PropertyName;

            if (jsonElement.IsNullOrUndefined())
            {
                if (propertyIsNullable)
                    return new JsonElement(); // undefined

                var subPathEnd = offset + propertyName.Length;
                throw new KeyNotFoundException($"Property not found at path {path[0..subPathEnd]}");
            }

            var success = jsonElement.ValueKind switch
            {
                JsonValueKind.Object => jsonElement.TryGetProperty(propertyName, out jsonElement),
                JsonValueKind.Array => TryGetArrayItem(ref jsonElement, ref propertyName, out jsonElement),
                _ => false,
            };

            if (!success && pathSection.NextPropertyIsNullable)
                return jsonElement;

            propertyIsNullable = pathSection.NextPropertyIsNullable;
            offset = pathSection.EndIndex;
        }
        while (offset < path.Length);

        return jsonElement;
    }

    private static PathSection GetPathSection(ref string fullPath, int startOffset)
    {
        var endIndex = startOffset;
        var nextPropertyIsNullable = false;
        char ? character;

        do
        {
            ++endIndex;

            if (endIndex == fullPath.Length)
                break;

            character = fullPath[endIndex];

            if (character != Dot)
                nextPropertyIsNullable = character == QuestionMark;
        }
        while (character != Dot);

        var nameEnd = nextPropertyIsNullable ? endIndex - 1 : endIndex;
        var propertyName = fullPath[startOffset..nameEnd];

        return new PathSection(propertyName, endIndex + 1, nextPropertyIsNullable);
    }

    private static bool TryGetArrayItem(ref JsonElement el, ref string indexAsString, out JsonElement result)
    {
        if (!int.TryParse(indexAsString, out var index) || index < 0)
        {
            result = new JsonElement();
            return false;
        }

        var array = el.EnumerateArray();
        result = array.ElementAtOrDefault(index);
        return true;
    }
}
