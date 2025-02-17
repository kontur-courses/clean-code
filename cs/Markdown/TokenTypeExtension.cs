namespace Markdown;

public static class TokenTypeExtension
{
    private static readonly Dictionary<TokenType, string> ValueTokenTypeMd = new()
    {
        { TokenType.Bold, "__" },
        { TokenType.Italic, "_" }
    };


    private static readonly List<Func<char, bool>> DetectType = [char.IsDigit, char.IsWhiteSpace];

    private static readonly Dictionary<Func<char, bool>, TokenType> TokenTypes =
        new() // experiments (вывести в другой класс) 
        {
            { char.IsDigit, TokenType.Digit },
            { char.IsWhiteSpace, TokenType.WhiteSpace },
        };

    private static readonly Dictionary<TokenType, Func<string, bool>> PossibleCharTokenType =
        new()
        {
            {
                TokenType.Word,
                c => c[0] != '_' && c[0] != '\n' && c[0] != ' ' && c[0] != '\\' && !char.IsDigit(c[0])
            },
            { TokenType.Digit, c => char.IsDigit(c[0]) },
            { TokenType.WhiteSpace, c => char.IsWhiteSpace(c[0]) },
            { TokenType.Italic, c => c[0] == '_' },
            { TokenType.NewLine, c => c[0] == '\n' },
            { TokenType.BackSlash, c => c[0] == '\\' },
            { TokenType.Header, c => c[0] == '#' && c[1] == ' ' },
            { TokenType.Bold, c => c[0] == '_' && c[1] == '_' },
            { TokenType.Marker, c => c[0] == '*' && c[1] == ' ' }
        };

    public static Token CreateTokenMd(this TokenType type, int ptr) =>
        new(ValueTokenTypeMd[type], type) { StartIndex = ptr };


    public static bool IsMatchMd(this TokenType type, string c) =>
        PossibleCharTokenType.TryGetValue(type, out var check) && check(c);


    public static TokenType AsSimpleTokenType(this char c)
    {
        foreach (var isTypeMatch in DetectType.Where(isTypeMatch => isTypeMatch(c)))
        {
            return TokenTypes[isTypeMatch];
        }

        return TokenType.Word;
    }


    public static bool IsMatchMd(this TokenType type, char c) => IsMatchMd(type, c.ToString());
}