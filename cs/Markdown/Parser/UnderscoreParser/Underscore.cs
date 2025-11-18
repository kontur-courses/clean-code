namespace Markdown;

public record Underscore(
    bool IsStrong,
    int TokenIndex,
    int NodeIndex,
    int ContentStartIndex,
    bool IsSuppressed
);