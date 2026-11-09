namespace Markdown;

public record Marker(
    string Value,
    int StartIndex,
    int EndIndex // не вкл
    );