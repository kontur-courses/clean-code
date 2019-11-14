using System.Collections.Generic;
using Markdown.Languages;

namespace Markdown.Tree
{
    public class TextNode : SyntaxNode
    {
        public readonly string Value;
        public TextNode(string text) : base(null)
        {
            Value = text;
        }
        
        public override string ConvertTo(Dictionary<TagType, Tag> tags)
        {
            if (Value == null)
            {
                return "";
            }
            return Value;
        }
    }
}