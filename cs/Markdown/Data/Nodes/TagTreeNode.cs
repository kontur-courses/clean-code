using Markdown.Data.TagsInfo;

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
    }
}