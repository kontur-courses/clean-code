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

    private char Current
    {
        get
        {
            if (position >= text.Length)
                return '\0';
            return text[position];
        }
    }

    public IEnumerable<Token> GetTokens()
    {
        //TODO: переделать условие
        while (Current != '\0'){
            if (char.IsWhiteSpace(Current))
            {
                var start = position;
                while (char.IsWhiteSpace(Current))
                    position++;

                var length = position - start;
                var tokenText = text.Substring(start, length);
                yield return new Token(SyntaxKind.Whitespace, start, tokenText);
            }
            //TODO: case
            if (Current == '_')
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
            else
            {
                var start = position;
                var letters = new List<char>();
                if (Current == '\\') 
                    position++;
                letters.Add(Current);
                position++;
                while (char.IsLetter(Current))
                {
                    if (Current == '\\')
                        position++;
                    letters.Add(Current);
                    position++;
                }
                yield return new Token(SyntaxKind.Text, start, new string(letters.ToArray()));
            }
        }
    }
}