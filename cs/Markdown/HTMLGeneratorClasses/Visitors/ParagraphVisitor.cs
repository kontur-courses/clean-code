using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Markdown.ParserClasses.Nodes;

namespace Markdown.HTMLGeneratorClasses.Visitors
{
    class ParagraphVisitor
    {
        public string Visit(ParagraphNode paragraphNode)
        {
            var sentenceVisitor = new SentenceVisitor();
            var sentences = paragraphNode.Sentences.Select(sentenceVisitor.Visit);
            var paragraph = string.Join("", sentences);

            return $"<p>{paragraph}</p>";
        }
    }
}
