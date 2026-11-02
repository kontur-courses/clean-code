using Markdown.Nodes;

namespace Markdown.Parsing;

public class InlineParser
{
    public List<INode> Parse(string text)
    {
        throw new NotImplementedException();
    }

    private List<Token> Tokenize(string text)
    {
        throw new NotImplementedException();
    }

    private char? TryEscape(char nextChar)
    {
        throw new NotImplementedException();
    }

    private bool IsValidMarkerContext(string text, int index, int length)
    {
        throw new NotImplementedException();
    }

    private HashSet<(int Index, bool IsOpening)> FindValidPairs(List<Token> tokens, string text)
    {
        throw new NotImplementedException();
    }
}