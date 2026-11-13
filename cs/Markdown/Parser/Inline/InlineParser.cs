using System;
using System.Collections.Generic;
using System.Text;
using Markdown.Entity;
using Markdown.Parser.Inline.Tokens;

namespace Markdown.Parser.Inline
{
    public static class InlineParser
    {
        public static List<INode> GetTagsTree(string text)
        {
            return Pairer.Pairing(Tokenizer.Tokenize(text));
        }
    }
}
