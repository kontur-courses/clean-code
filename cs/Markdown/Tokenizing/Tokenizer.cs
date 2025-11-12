using System.Text;

namespace Markdown.Tokenizing;

public class Tokenizer
{
    public static List<Token> Tokenize(string text)
    {
        ArgumentNullException.ThrowIfNull(text);

        var cursor = new CharCursor(text);
        var tokens = new List<Token>();

        while (!cursor.End)
        {
            var c = cursor.Current;

            switch (c)
            {
                case '\n':
                    tokens.Add(new Token(TokenType.EndOfLine, "\n"));
                    cursor.MoveNext();
                    break;

                case ' ':
                case '\t':
                    tokens.Add(new Token(TokenType.Whitespace, c.ToString()));
                    cursor.MoveNext();
                    break;

                case '\\':
                    tokens.Add(new Token(TokenType.Escape, "\\"));
                    cursor.MoveNext();
                    break;

                case '_':
                    if (cursor.MatchNext('_'))
                    {
                        tokens.Add(new Token(TokenType.DoubleUnderscore, "__"));
                        cursor.MoveNext();
                        cursor.MoveNext();
                    }
                    else
                    {
                        tokens.Add(new Token(TokenType.Underscore, "_"));
                        cursor.MoveNext();
                    }

                    break;

                case '#':
                    tokens.Add(new Token(TokenType.Hash, "#"));
                    cursor.MoveNext();
                    break;

                case '[':
                    tokens.Add(new Token(TokenType.LeftBracket, "["));
                    cursor.MoveNext();
                    break;

                case ']':
                    tokens.Add(new Token(TokenType.RightBracket, "]"));
                    cursor.MoveNext();
                    break;

                case '(':
                    tokens.Add(new Token(TokenType.LeftParen, "("));
                    cursor.MoveNext();
                    break;

                case ')':
                    tokens.Add(new Token(TokenType.RightParen, ")"));
                    cursor.MoveNext();
                    break;

                default:
                    ReadText(tokens, cursor);
                    break;
            }
        }

        tokens.Add(new Token(TokenType.EndOfFile, ""));
        return tokens;
    }

    private static void ReadText(List<Token> tokens, CharCursor cursor)
    {
        var sb = new StringBuilder();

        while (!cursor.End && IsTextChar(cursor.Current))
        {
            sb.Append(cursor.Current);
            cursor.MoveNext();
        }

        if (sb.Length > 0)
            tokens.Add(new Token(TokenType.Text, sb.ToString()));
    }

    private static bool IsTextChar(char c)
    {
        return c != '_' &&
               c != '\\' &&
               c != '[' &&
               c != ']' &&
               c != '(' &&
               c != ')' &&
               c != '#' &&
               c != '\n' &&
               !char.IsWhiteSpace(c);
    }
}