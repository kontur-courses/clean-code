using System.Collections.Immutable;

namespace Markdown;

public class Lexer
{
    private readonly string text;

    private const string specialSymbols = "_#()[]\n\t ";
    private const string escapableSymbols = "_#()[]\\";

    private int position;

    public Lexer(string text)
    {
        this.text = text;
        position = 0;
    }

    private char Peek(int offset)
    {
        var index = position + offset;
        if (index >= text.Length)
            return '\0';
        return text[index];
    }

    private char Current => Peek(0);
    private char Next => Peek(1);

    public IEnumerable<Token> GetTokens()
    {
        while (Current != '\0')
        {
            switch (Current)
            {
                case '\n':
                    yield return new Token(SyntaxKind.NewLine, position, "\n");
                    break;
                case ' ':
                case '\t':
                {
                    var start = position;
                    while (Next == ' ' || Next == '\t')
                        position++;

                    var length = position - start + 1;
                    var tokenText = text.Substring(start, length);
                    yield return new Token(SyntaxKind.Whitespace, start, tokenText);
                    break;
                }
                case '[':
                    yield return new Token(SyntaxKind.OpenSquareBracket, position, "[");
                    break;
                case ']':
                    yield return new Token(SyntaxKind.CloseSquareBracket, position, "]");
                    break;
                case '(':
                    yield return new Token(SyntaxKind.OpenRoundBracket, position, "(");
                    break;
                case ')':
                    yield return new Token(SyntaxKind.CloseRoundBracket, position, ")");
                    break;
                case '#':
                    yield return new Token(SyntaxKind.Hash, position, "#");
                    break;
                case '_':
                {
                    var start = position;
                    if (Next == '_')
                    {
                        position++;
                        yield return new Token(SyntaxKind.DoubleUnderscore, start, "__");
                    }
                    else
                        yield return new Token(SyntaxKind.SingleUnderscore, start, "_");

                    break;
                }
                default:
                {
                    var start = position;
                    var letters = new List<char>();
                    while (Current != '\0' && (!specialSymbols.Contains(Current) || Current == '\\'))
                    {
                        if (Current == '\\' && escapableSymbols.Contains(Next))
                            position++;

                        letters.Add(Current);
                        position++;
                    }

                    yield return new Token(SyntaxKind.Text, start, new string(letters.ToArray()));
                    continue;
                }
            }

            position++;
        }
    }
}