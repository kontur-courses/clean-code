using Markdown.Tokens;

namespace Markdown.Tags;

public class ItalicTag : TextTag
{
    public override TagType TagType => TagType.Italic;
    protected override string TagContent => "_";

    public override bool IsStartOfTag(string content) => 
        IsTag(content);
}