using Markdown.ParserClasses.Nodes;

namespace Markdown.HTMLGeneratorClasses
{
    public class HTMLGenerator
    {
        public string Generate(SentenceNode abstractSyntaxTree)
        {
            return SentenceBuilder.Build(abstractSyntaxTree);
        }
    }
}