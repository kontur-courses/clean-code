namespace Markdown;

public static class SymbolStatusParser
{
    public static SymbolStatus ParseSymbolStatus(char symbol)
    {
        if (char.IsLetter(symbol))
            return SymbolStatus.text;
        if (char.IsDigit(symbol))
            return SymbolStatus.digit;
        if (symbol == ' ')
            return SymbolStatus.space;
        if (symbol == '_')
            return SymbolStatus.underscore;
        if (symbol == '\\')
            return SymbolStatus.backslash;
        if (symbol == '\n')
            return SymbolStatus.newline;
        if (symbol == '\0')
            return SymbolStatus.eof;
        if (symbol == '#')
            return SymbolStatus.sharp;
        return SymbolStatus.none;
    }
}