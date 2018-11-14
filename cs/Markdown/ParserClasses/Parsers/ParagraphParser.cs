using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Markdown.ParserClasses.Nodes;
using Markdown.TokenizerClasses;

namespace Markdown.ParserClasses.Parsers
{
    public class ParagraphParser
    {
        public ParagraphNode Parse(TokenList tokens)
        {
            var sentences = Utils.ParseMultiple(tokens, new SentenceParser().Parse);
            var nodes = sentences.Item1;
            var consumed = sentences.Item2;

            return nodes.Count == 0 ? null : new ParagraphNode(nodes, consumed);
        }
    }
}
