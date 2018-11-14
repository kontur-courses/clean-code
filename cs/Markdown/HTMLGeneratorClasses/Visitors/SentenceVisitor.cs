using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Markdown.HTMLGeneratorClasses.Visitors.TextVisitors;
using Markdown.ParserClasses.Nodes;

namespace Markdown.HTMLGeneratorClasses.Visitors
{
    class SentenceVisitor
    {
        public static readonly Dictionary<string, Func<Node, string>> sentenceVisitors =
            new Dictionary<string, Func<Node, string>>
            {
                {"BOLD", new BoldTextVisitor().Visit},
                {"EMPHASIS", new EmphasisTextVisitor().Visit},
                {"TEXT", new PlainTextVisitor().Visit}
            };

        public string Visit(Node sentence)
        {
            return sentenceVisitors[sentence.Type](sentence);
        }
    }
}
