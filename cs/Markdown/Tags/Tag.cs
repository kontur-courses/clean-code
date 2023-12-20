using System.Reflection;

namespace Markdown.Tags;

public abstract class Tag
{
    public bool IsPaired = false;
    public string ReplacementForOpeningTag { get; set; } = null!;
    public string ReplacementForClosingTag = null!; 
    public string TagContent { get; set; } = null!;

    public TagStatus Status = TagStatus.Undefined;
    public TagType TagType {  get; set; }
    public TagType[] ExcludedTags = null!;
    protected Token PreviousToken;
    protected abstract Tag CreateTag(string content, Token previousToken, string nextChar);
    
    public static Tag CreateTag(TagType type, string content, Token previousToken, string nextChar)
    {
        var tokenType = Assembly.GetExecutingAssembly().GetType($"Markdown.Tags.{type}");
        if (tokenType == null || !typeof(Tag).IsAssignableFrom(tokenType))
        {
            throw new ArgumentException("Unsupported token type", nameof(type));
        }
        
        var token = (Tag)Activator.CreateInstance(tokenType)!;
        return token.CreateTag(content, previousToken, nextChar);
    }
}