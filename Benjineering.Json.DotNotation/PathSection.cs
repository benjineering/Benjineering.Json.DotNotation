namespace Benjineering.Json.DotNotation;

internal class PathSection
{
    public string PropertyName { get; }
    public int EndIndex { get; }
    public bool NextPropertyIsNullable { get; }

    public PathSection(string propertyName, int endIndex, bool nextPropertyIsNullable)
    {
        PropertyName = propertyName;
        EndIndex = endIndex;
        NextPropertyIsNullable = nextPropertyIsNullable;
    }
}
