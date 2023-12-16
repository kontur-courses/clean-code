namespace Markdown;

public class Lexer
{
    private readonly string text;
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

    public IEnumerable<Token> GetTokens()
    {
        while (Current != '\0')
        {
            if (Current == '\n')
            {
                yield return new Token(SyntaxKind.NewLine, position, "\n");
                position++;
            }
            else if (char.IsWhiteSpace(Current))
            {
                var start = position;
                while (char.IsWhiteSpace(Current))
                    position++;

                var length = position - start;
                var tokenText = text.Substring(start, length);
                yield return new Token(SyntaxKind.Whitespace, start, tokenText);
            }
            else if (Current == '_')
            {
                var start = position;
                position++;
                if (Current == '_')
                {
                    position++;
                    yield return new Token(SyntaxKind.DoubleUnderscore, start, "__");
                }
                else
                {
                    yield return new Token(SyntaxKind.SingleUnderscore, start, "_");
                }
            }
            else if (Current == '#')
            {
                yield return new Token(SyntaxKind.Hash, position, "#");
                position++;
            }
            else
            {
                var start = position;
                var letters = new List<char>();
                while (Current != '_' && Current != '#' && Current != ' ' && Current != '\0' && Current != '\n' ||
                       Current == '\\')
                {
                    if (Current == '\\')
                    {
                        if (Peek(1) == '_' || Peek(1) == '\\')
                            position++;
                    }

                    letters.Add(Current);
                    position++;
                }

                yield return new Token(SyntaxKind.Text, start, new string(letters.ToArray()));
            }
        }
    }
}