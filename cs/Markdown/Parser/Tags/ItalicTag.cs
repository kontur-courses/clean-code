using Markdown.Tree;

namespace Markdown.Parser.Tags
{
    public class ItalicTag : MarkdownTag
    {
        public override string String => "_";
        public override Node CreateNode() => new ItalicNode();
    }
}