namespace Markdown.Markdown.Tags;

public abstract class TagValidatorBase(string tagMarker) : ITagValidator 
{
    private readonly int tagLength = tagMarker.Length; 
 
    public bool IsValidTag(string content) => 
        HasValidLength(content) && 
        HasValidTagMarkers(content); 
 
    public bool IsTagStart(string content) => 
        !string.IsNullOrEmpty(content) && content.StartsWith(tagMarker[0]);
    
    public bool IsTagEnd(string content) => 
        !string.IsNullOrEmpty(content) && content.EndsWith(tagMarker[^1]); 
 
    private bool HasValidLength(string content) => 
        content.Length == tagLength; 
 
    private bool HasValidTagMarkers(string content) => 
        content.StartsWith(tagMarker) && content.EndsWith(tagMarker); 
}