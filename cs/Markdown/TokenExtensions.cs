namespace Markdown;

public static class TokenExtensions
{
    public static void RemovePair(this Token? token)
    {
        if (token?.Pair == null) return;
        
        token.Pair.Pair = null;
        token.Pair = null;
    }
}