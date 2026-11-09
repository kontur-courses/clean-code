namespace Markdown.Tokenizer.Tokens;

public record Token(
    TokenType Type,
    string Value,
    int Position,
    bool CanOpen = false,
    bool CanClose = false,
    bool IsInsideWord = false
);