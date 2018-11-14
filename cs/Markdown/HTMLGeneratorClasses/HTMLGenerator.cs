using System;
using Markdown.HTMLGeneratorClasses.Visitors;
using Markdown.ParserClasses.Nodes;

namespace Markdown.HTMLGeneratorClasses
{
    public class HTMLGenerator
    {
        public string Generate(ContentNode abstractSyntaxTree)
        {
            return new ContentVisitor().Visit(abstractSyntaxTree);
        }
    }
}
