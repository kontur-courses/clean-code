using Markdown.Tags.ContextRules;

namespace Markdown.Tags;

public class Italic(string mdText, int tagStart) : PairTag(mdText, tagStart)
{
    protected override string MdTag => "_";
    protected override string HtmlTag => "em";
    public override MdTagType TagType => MdTagType.Italic;
}