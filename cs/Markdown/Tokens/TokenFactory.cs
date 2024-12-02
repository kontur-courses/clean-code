using System.Text;
using Markdown.Enums;

namespace Markdown.Tokens;

public static class TokenFactory
{
    private static readonly string[] TagSymbols =
        [SeparatorType.Screening, SeparatorType.Underscore, SeparatorType.Space];
    
    public static Token GenerateToken(string text, int index)
    {
        string separator;
        if (index + 1 < text.Length)
        {
            separator = text.Substring(index, 2);
            switch (separator)
            {
                case SeparatorType.DoubleUnderscore:
                    return CreateBoldToken(separator);
                case SeparatorType.Hash:
                    if (index > 0) break; 
                    return CreateParagraphToken(separator);
            }
        }
        separator = text[index].ToString();
        return separator switch
        {
            SeparatorType.Underscore => CreateItalicsToken(separator),
            SeparatorType.Screening => CreateScreeningToken(separator),
            SeparatorType.Space => CreateSpaceToken(separator),
            _ => CreateLiteralToken(text, index)
        };
    }
    
    private static BoldToken CreateBoldToken(string text) => new(text);
    
    private static ItalicsToken CreateItalicsToken(string text) => new(text);
    
    private static ScreeningToken CreateScreeningToken(string text) => new(text);
    
    private static SpaceToken CreateSpaceToken(string text) => new(text);
    
    private static ParagraphToken CreateParagraphToken(string text) => new(text);

    private static LiteralToken CreateLiteralToken(string text, int index)
    {
        var literalType = LiteralType.None;
        var content = new StringBuilder();
        for (var i = index; i < text.Length; i++)
        {
            if (i == index)
                literalType = char.IsNumber(text[i]) ? LiteralType.Number : LiteralType.Text;
            if (IsTextEnd(literalType, text[i]) || IsNumberEnd(literalType, text[i]))
                break;
            content.Append(text[i]);
        }

        return new LiteralToken(content.ToString(), literalType);
    }

    private static bool IsTextEnd(LiteralType type, char symbol)
        => type == LiteralType.Text && (char.IsNumber(symbol) || TagSymbols.Any(s => s.StartsWith(symbol)));
    
    private static bool IsNumberEnd(LiteralType type, char symbol)
        => type == LiteralType.Number && !char.IsNumber(symbol);
}