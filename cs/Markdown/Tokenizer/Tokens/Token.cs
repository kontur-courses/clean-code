namespace Markdown.Tokenizer.Tokens;

public record Token(
    TokenType Type,
    string Value,
    bool CanOpen = false,
    bool CanClose = false,
    bool IsInsideWord = false
);