using System.Collections.Generic;
using System.Linq;
using System.Text;
using Markdown.Languages;

namespace Markdown.Tree
{
    public class TagNode : SyntaxNode, INode
    {
        public TagType TypeTag { get; }

        public TagNode(TagType tagType, List<SyntaxNode> childNode) : base(childNode)
        {
            TypeTag = tagType;
        }

        public TagNode(TagType tagType) : base(new List<SyntaxNode>())
        {
            TypeTag = tagType;
        }

        public void Add(SyntaxNode node)
        {
            ChildNode.Add(node);
        }

        public override string BuildLinesWithTag(LanguageTagDict languageTagDict)
        {
            var result = new StringBuilder();
            result.Append(languageTagDict[TypeTag].Start);
            foreach (var childText in ChildNode.Select(child => child.BuildLinesWithTag(languageTagDict))
                .Where(childText => childText != null))
            {
                result.Append(childText);
            }

            result.Append(languageTagDict[TypeTag].End);
            return result.ToString();
        }
    }
}