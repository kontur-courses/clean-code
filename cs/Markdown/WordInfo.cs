namespace Markdown;

// непрерывный участок между пробелами
public record WordInfo(
    int StartIndex,
    int EndIndex, // не вкл
    bool ContainsDigits
    );