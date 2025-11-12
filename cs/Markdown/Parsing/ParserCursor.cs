using Markdown.Tokenizing;

namespace Markdown.Parsing;

public class ParserCursor(IList<Token> tokens)
{
    private int index;
    public bool End => index >= tokens.Count - 1;
    public int Index => index;
    public Token Current => !End ? tokens[index] : tokens[^1];

    public Token Peek()
    {
        var nextIndex = index + 1;
        return nextIndex < tokens.Count ? tokens[nextIndex] : tokens[^1];
    }

    public void MoveNext()
    {
        if (!End) index++;
    }

    public void IndexJumpTo(int newIndex)
    {
        if (newIndex < 0) newIndex = 0;

        if (newIndex >= tokens.Count) newIndex = tokens.Count - 1;

        index = newIndex;
    }
}