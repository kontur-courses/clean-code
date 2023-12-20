using System.Reflection;

namespace Markdown.Tags;

public abstract class Tag
{
    public bool IsPaired = false;
    public string ClosingTag; 
    public TagType[] BlockTags;
    public char NextChar;
    public string ConvertTo { get; set; }
    public string Content { get; set; }

    public TagStatus Status = TagStatus.Undefined;
    public TagType TokenType {  get; set; }
    public Token PreviousToken;
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