using System.Collections.Generic;
using Markdown.Languages;

namespace Markdown.Tree
{
    public class TextNode : SyntaxNode
    {
        public string Value { get; }

        public TextNode(string text) : base(null)
        {
            Value = text;
        }

        public override string ConvertTo(LanguageTagDict languageTagDict)
        {
            return Value ?? "";
        }
    }
}