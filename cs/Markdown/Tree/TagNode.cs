using System.Collections.Generic;
using System.Text;
using Markdown.Languages;

namespace Markdown.Tree
{
    public class TagNode: SyntaxNode
    {
        public readonly TagType TypeTag;
        public TagNode(TagType tagType,List<SyntaxNode> childNode) : base(childNode)
        {
            TypeTag = tagType;
        }
        
        public void Add(SyntaxNode node)
        {
            ChildNode.Add(node);
        }

        public override string ConvertTo(ILanguage language)
        {
            var result = new StringBuilder();
            result.Append(language.Tags[TypeTag].Start);
            foreach (var child in ChildNode)
            {
                var childText = child.ConvertTo(language);
                if (childText != null)
                {
                    result.Append(childText);
                }
            }
            result.Append(language.Tags[TypeTag].End);
            return result.ToString();
        }
    }
}