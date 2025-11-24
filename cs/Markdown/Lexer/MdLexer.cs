using System.Text;
using Markdown.Domains;

namespace Markdown.Lexer;

/// <summary>
///     Разбивает входной текст на последовательность Md-токенов (<see cref="TokenType"/>).
/// </summary>
/// <remarks>
///     Поддерживаемые типы токенов:
///     <list type="bullet">
///         <item><description><see cref="TokenType.Word"/> — последовательность буквенных символов.</description></item>
///         <item><description><see cref="TokenType.Number"/> — последовательность цифр.</description></item>
///         <item><description><see cref="TokenType.Space"/> — пробельный символ пробела.</description></item>
///         <item><description>
///             <see cref="TokenType.Tab"/> — символ табуляции, который при разборе
///             заменяется на <see cref="MdLexer.SpacesCountInTab"/> пробелов.
///         </description></item>
///         <item><description><see cref="TokenType.Underscore"/> — символ подчёркивания (<c>_</c>).</description></item>
///         <item><description><see cref="TokenType.Grid"/> — символ решётки (<c>#</c>).</description></item>
///         <item><description><see cref="TokenType.Escape"/> — символ экранирования (<c>\</c>).</description></item>
///         <item><description><see cref="TokenType.Slash"/> — слэш (<c>/</c>).</description></item>
///         <item><description><see cref="TokenType.LeftSquareBracket"/> — левая квадратная скобка (<c>[</c>).</description></item>
///         <item><description><see cref="TokenType.RightSquareBracket"/> — правая квадратная скобка (<c>]</c>).</description></item>
///         <item><description><see cref="TokenType.LeftParenthesis"/> — левая круглая скобка (<c>(</c>).</description></item>
///         <item><description><see cref="TokenType.RightParenthesis"/> — правая круглая скобка (<c>)</c>).</description></item>
///     </list>
/// </remarks>
public static class MdLexer
{
    private static readonly Dictionary<char, TokenType> TokenMap = new()
    {
        { '#', TokenType.Grid },
        { '_', TokenType.Underscore },
        { ' ', TokenType.Space },
        { '\u00a0', TokenType.Space },
        { '\u200b', TokenType.Space },
        { '\t', TokenType.Space },
        { '\n', TokenType.Escape },
        { '\r', TokenType.Escape },
        { '\\', TokenType.Slash },
        { '[', TokenType.LeftSquareBracket },
        { ']', TokenType.RightSquareBracket },
        { '(', TokenType.LeftParenthesis },
        { ')', TokenType.RightParenthesis }
    };

    public static List<MdToken> Tokenize(string text)
    {
        var tokens = new List<MdToken>();

        for (var i = 0; i < text.Length; i++)
        {
            var symbol = text[i];
            var tokenType = GetTokenType(symbol);

            switch (tokenType)
            {
                case TokenType.Word or TokenType.Number:
                    var (value, nextIndex) = CollectFullValue(text, i,
                        tokenType is TokenType.Word ? IsPieceOfWord : char.IsNumber
                    );
                    tokens.Add(new MdToken(tokenType, value));
                    i = nextIndex;
                    break;
                default:
                    tokens.Add(new MdToken(tokenType, symbol.ToString()));
                    break;
            }
        }

        return tokens;
    }

    public static TokenType GetTokenType(char text)
    {
        if (TokenMap.TryGetValue(text, out var tokenType))
            return tokenType;

        return char.IsNumber(text) ? TokenType.Number : TokenType.Word;
    }

    private static (string word, int nextIndex) CollectFullValue(string text, int startIndex,
        Func<char, bool> predicate)
    {
        var value = new StringBuilder();
        value.Append(text[startIndex]);

        var i = startIndex + 1;
        while (i < text.Length && predicate(text[i]))
        {
            value.Append(text[i]);
            i++;
        }

        return (value.ToString(), i - 1);
    }

    private static bool IsPieceOfWord(this char ch)
    {
        return char.IsLetter(ch) || !TokenMap.ContainsKey(ch);
    }
}