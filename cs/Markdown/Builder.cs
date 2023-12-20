using System.Reflection.Metadata;
using System.Text;

namespace Markdown;

public static class Builder
{
    private static bool InTag;
    private static TagType[] blockTags = Array.Empty<TagType>();
    public static string Build(List<Token> tokens)
    {
        var text = new StringBuilder();
        foreach (var token in tokens)
        {
            if (token.Type == TokenType.Tag && !blockTags.Contains(token.Tag.TokenType))
            {
                HandleTags(token);
                text.Append(token.Tag.Content);
            }
            else text.Append(token.Content);
        }
        return text.ToString();
    }

    private static void HandleTags(Token token)
    {
        if (token.Tag.Content == token.Tag.ConvertTo)
        {
            blockTags = token.Tag.BlockTags != null ? token.Tag.BlockTags : new TagType[]{};
        }
        else if(token.Tag.Content == token.Tag.ClosingTag)
        {
            blockTags = new TagType[]{};
        }
    }
}