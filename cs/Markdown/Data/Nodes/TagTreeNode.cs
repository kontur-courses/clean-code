using System.Text;
using Markdown.Data.TagsInfo;
using Markdown.TreeTranslator.NodeTranslator;

namespace Markdown.Data.Nodes
{
    public class TagTreeNode : TokenTreeNode
    {
        public readonly ITagInfo TagInfo;
        public bool IsRaw;

        public TagTreeNode(ITagInfo tagInfo)
        {
            TagInfo = tagInfo;
        }

        public override void Translate(INodeTranslator translator, StringBuilder textBuilder) =>
            translator.Translate(this, textBuilder);
    }
}