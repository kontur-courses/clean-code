using Markdown.Parser;
using Markdown.Tokens.StringToken;

namespace Markdown;

public class MarkdownParser() : IParser
{
    private int position;
    private string text;
    private char Peek(int offset)
    {
        var index = position + offset;
        return index >= text.Length ? '\0' : text[index];
    }

    private char Current => Peek(0);
    private char Next => Peek(1);
    private static bool IsMarkupSymbol(char c) => @"_#\".Contains(c);

    public IEnumerable<StringToken> Parse(string text)
    {
        this.text = text;
        while (Current != '\0'){
            switch (Current)
            {
                case '\n':
                case '\r':
                    yield return new StringToken(Current.ToString(), position, StringTokenType.NewLine);
                    break;
                case ' ':
                case '\t':
                {
                    var start = position;
                    while (Next == ' ' || Next == '\t')
                        position++;

                    var length = position - start + 1;
                    var tokenText = text.Substring(start, length);
                    yield return new StringToken(tokenText, start, StringTokenType.WhiteSpace);
                    break;
                }
                case '#':
                    yield return new StringToken("#", position, StringTokenType.Hash);
                    break;
                case '_':
                {
                    var start = position;
                    if (Next == '_')
                    {
                        position++;
                        yield return new StringToken("__", start, StringTokenType.DoubleUnderscore);
                    }
                    else
                        yield return new StringToken("_", start, StringTokenType.SingleUnderscore);

                    break;
                }
                case '-':
                {
                    yield return new StringToken("-", position, StringTokenType.Dash);
                    break;
                }
                default:
                {
                    if (!char.IsLetter(Current) && Current != '\\')
                    {
                        yield return new StringToken(Current.ToString(), position, StringTokenType.Unexpected);
                        break;
                    }

                    var start = position;
                    var letters = new List<char>();
                    while (Current != '\0' && (char.IsLetter(Current) || Current == '\\'))
                    {
                        if (Current == '\\' && IsMarkupSymbol(Next))
                            position++;

                        letters.Add(Current);
                        position++;
                    }

                    yield return new StringToken(new string(letters.ToArray()), start, StringTokenType.Text);
                    continue;
                }
            }

            position++;
        }
    }
}