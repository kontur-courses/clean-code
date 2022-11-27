using Markdown.Tags;

namespace Markdown.Extensions;

public static class TokenExtension
{
    public static bool CheckAndUpdateTokenIfInDifferentWords(this Token token, char symbol)
    {
        if (!char.IsWhiteSpace(symbol)) return false;
        if (token.Parent == null || !Tag.IsHighlightingTag(token.Parent.Tag)) return false;

        token.Parent.Childrens.Add(token);
        token.Parent.ToTextToken();
            
        return true;
    }
}