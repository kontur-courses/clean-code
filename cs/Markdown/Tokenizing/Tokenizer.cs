using System.Text;
using Markdown.Tokenizing.Tokens;

namespace Markdown.Tokenizing;

public class Tokenizer
{
    private readonly HashSet<char> SpecialSymbols = ['\\', '_', '#', '[', ']', '(', ')', ' '];

    private Token ParseDefaultText(Cursor cursor)
    {
        var sb = new StringBuilder();
        while (!cursor.IsEndOfText && !SpecialSymbols.Contains(cursor.CurrentChar))
        {
            sb.Append(cursor.CurrentChar);
            cursor.MoveForward();
        }

        return new Token { Type = TokenType.Text, Value = sb.ToString() };
    }

    /// <summary>
    /// Разбивает текст на набор токенов
    /// </summary>
    /// <param name="text">Текст для разбивания</param>
    /// <returns></returns>
    public List<Token> Tokenize(string text)
    {
        var cursor = new Cursor(text);
        var result = new List<Token>();

        while (!cursor.IsEndOfText)
        {
            var currentChar = cursor.CurrentChar;

            switch (currentChar)
            {
                case '\\':
                    result.Add(new Token { Type = TokenType.EscapeCharacter });
                    cursor.MoveForward();
                    break;
                case '_':
                    result.Add(new Token { Type = TokenType.Underscore });
                    cursor.MoveForward();
                    break;
                case '#':
                    result.Add(new Token { Type = TokenType.Hashtag });
                    cursor.MoveForward();
                    break;
                case '[':
                    result.Add(new Token { Type = TokenType.LeftSquareBracket });
                    cursor.MoveForward();
                    break;
                case ']':
                    result.Add(new Token { Type = TokenType.RightSquareBracket });
                    cursor.MoveForward();
                    break;
                case '(':
                    result.Add(new Token { Type = TokenType.LeftBracket });
                    cursor.MoveForward();
                    break;
                case ')':
                    result.Add(new Token { Type = TokenType.RightBracket });
                    cursor.MoveForward();
                    break;
                case ' ':
                    result.Add(new Token { Type = TokenType.WhiteSpace });
                    cursor.MoveForward();
                    break;

                default:
                    result.Add(ParseDefaultText(cursor));
                    break;
            }
        }

        return result;
    }
}