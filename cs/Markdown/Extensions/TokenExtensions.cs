namespace Markdown.Extensions;

public static class TokenExtensions
{
    private static TagType[] blockTags = Array.Empty<TagType>();

    public static bool IsTagEligible(this Token token)
    {
        return token.Type == TokenType.Tag && !blockTags.Contains(token.Tag.TagType);
    }

    public static void HandleTags(this Token token)
    {
        if (token.Tag.IsOpeningTag())
            blockTags = token.Tag.ExcludedTags ?? Array.Empty<TagType>();
        else if (token.Tag.IsClosingTag()) blockTags = Array.Empty<TagType>();
    }
}