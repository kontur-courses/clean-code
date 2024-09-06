using System.Reflection;
using Markdown.Tokens;

namespace Markdown.Tags;

public abstract class Tag
{
    public abstract bool IsPaired { get; protected set; }
    public abstract string? ReplacementForOpeningTag { get; protected set; }
    public abstract string? ReplacementForClosingTag { get; protected set; }
    public abstract string? TagContent { get; set; }

    public abstract TagStatus Status { get; set; }
    public abstract TagType TagType { get; protected set; }
    public abstract TagType[] ExcludedTags { get; protected set; }
    protected Token? PreviousToken;
    protected abstract Tag CreateTag(string? content, Token? previousToken, string nextChar);

    public static Tag CreateTag(TagType type, string? content, Token? previousToken, string nextChar)
    {
        var tagTypeMap = new Dictionary<TagType, Func<Tag>>
        {
            { TagType.Bold, () => new Bold() },
            { TagType.Bulleted, () => new Bulleted() },
            { TagType.Italic, () => new Italic() },
            { TagType.Heading, () => new Heading() }
        };
        if (tagTypeMap.TryGetValue(type, out var createToken))
        {
            var token = createToken();
            return token.CreateTag(content, previousToken, nextChar);
        }
        else
        {
            throw new ArgumentOutOfRangeException(nameof(type), type, null);
        }
    }
}