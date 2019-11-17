﻿using System.Collections.Generic;

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
            return Value ?? "";
        }
    }
}