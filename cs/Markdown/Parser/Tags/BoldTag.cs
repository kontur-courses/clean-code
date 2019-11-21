using Markdown.Tree;

namespace Markdown.Parser.Tags
{
    public class BoldTag : MarkdownTag
    {
        public override string String => "__";
        public override Node Node => new BoldNode();
    }
}