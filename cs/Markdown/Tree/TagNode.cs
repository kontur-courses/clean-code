using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Markdown.Tree
{
    public class TagNode : SyntaxNode
    {
        public readonly TagType TypeTag;

        public TagNode(TagType tagType, List<SyntaxNode> childNode) : base(childNode)
        {
            TypeTag = tagType;
        }

        public void Add(SyntaxNode node)
        {
            ChildNode.Add(node);
        }

        public override string ConvertTo(Dictionary<TagType, Tag> tags)
        {
            var result = new StringBuilder();
            result.Append(tags[TypeTag].Start);
            foreach (var childText in ChildNode.Select(child => child.ConvertTo(tags))
                .Where(childText => childText != null))
            {
                result.Append(childText);
            }

            result.Append(tags[TypeTag].End);
            return result.ToString();
        }
    }
}