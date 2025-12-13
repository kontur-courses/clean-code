namespace Markdown.Tokenizer;

public enum TokenType
{
    Hash,
    Underscore,
    DoubleUnderscore,
    WordUnderscore,
    WordDoubleUnderscore,
    Text,
    NewLine,
    Exclamation,
    LBracket,
    RBracket,
    LParenthesis,
    RParenthesis,
    Escape,
    Space,
    Carriage,
    Eof
}