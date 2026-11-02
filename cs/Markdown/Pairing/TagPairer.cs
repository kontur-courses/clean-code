using Markdown.Tokens;

namespace Markdown.Pairing;

public class TagPairer
{
    public void Pair(List<Token> tokens)
    {
        throw new NotImplementedException();
    }

    private static bool CanOpen(Token token)
    {
        throw new NotImplementedException();
    }

    private static bool CanClose(Token token)
    {
        throw new NotImplementedException();
    }

    private static bool IsValidPair(List<Token> tokens, int openIndex, int closeIndex)
    {
        throw new NotImplementedException();
    }

    private static void DemoteStrongInsideEm(List<Token> tokens)
    {
        throw new NotImplementedException();
    }

    private static void DemoteToText(Token token)
    {
        throw new NotImplementedException();
    }
}