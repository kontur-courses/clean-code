using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Markdown.ParserClasses.Nodes;

namespace Markdown.HTMLGeneratorClasses.Visitors
{
    public class ContentVisitor
    {
        public string Visit(ContentNode content)
        {
            var paragraphVisitor = new ParagraphVisitor();
            var paragraphs = content.Paragraphs.Select(paragraphVisitor.Visit);
            var text = string.Join("\n\n", paragraphs);

            return text;
        }
    }
}
