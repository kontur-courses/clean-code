using System.Text;
using Markdown.Infrastructure.Formatters;
using Markdown.Infrastructure.Parsers;
using Markdown.Infrastructure.Parsers.Markdown;
using Ninject;

namespace Markdown
{
    public class Markdown
    {
        public string Render(string markdownText)
        {
            var container = new StandardKernel();
            container.Bind<string>().ToConstant(markdownText);
            container.Bind<IBlockBuilder>().To<BlockBuilder>();
            container.Bind<ITagValidator>().To<TagValidator>();
            container.Bind<ITagParser>().To<MarkdownParser>();
            
            var parser = container.Get<MarkdownParser>();
            var block = parser.Parse();

            var htmlFormatter = container.Get<HtmlFormatter>();
            var htmlSentences = block.Format(htmlFormatter);

            var stringBuilder = new StringBuilder();
            foreach (var htmlSentence in htmlSentences)
                stringBuilder.Append(htmlSentence);

            return stringBuilder.ToString();
        }
    }
}