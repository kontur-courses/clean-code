namespace Markdown;

public record struct UnderscoreMatch(
    Underscore? Underscore,
    int Index,
    bool IsIntersection
);