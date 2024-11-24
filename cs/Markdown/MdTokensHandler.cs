using Markdown.Tokens;

namespace Markdown;

public static class MdTokensHandler
{
    public static IEnumerable<IToken> HandleTokens(this LinkedList<IToken> tokens)
    {
        for (var token = tokens.First; token != null; token = token?.Next)
        {
            token.HandleToken();
        }

        tokens.HandleNesting();

        return tokens;
    }

    private static void HandleToken(this LinkedListNode<IToken> token)
    {
        switch (token.Value)
        {
            //TODO
        }
    }

    private static void HandleNesting(this LinkedList<IToken> tokens)
    {
        throw new NotImplementedException();
    }
}