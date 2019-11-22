using Markdown.Tree;

namespace Markdown.Parser.Tags
{
    public class ItalicTag : MarkdownTag
    {
        public override string String => "_";
        public override Node Node => new ItalicNode();
        public override string Name => "italic";
    }
}