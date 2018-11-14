using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Markdown.ParserClasses.Nodes;
using Markdown.TokenizerClasses;

namespace Markdown.ParserClasses.Parsers
{
    public class ContentParser
    {
        public ContentNode Parse(TokenList tokens)
        {
            var paragraphs = Utils.ParseMultipleParagraph(tokens, new ParagraphParser().Parse);
            var paragraphNodes = paragraphs.Item1;
            var consumed = paragraphs.Item2;

            return paragraphNodes.Count == 0 ? null : new ContentNode(paragraphNodes, consumed);
        }
    }
}
