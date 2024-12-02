using Markdown.Tokens;

namespace Markdown.Tags;

public class StrongTag : TextTag
{
    public override TagType TagType => TagType.Strong;
    protected override string TagContent => "__";

    public override bool IsStartOfTag(string content) => 
        content.Length != 0 && TagContent.StartsWith(content);
}