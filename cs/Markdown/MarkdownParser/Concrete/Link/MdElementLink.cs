using MarkdownParser.Infrastructure.Markdown.Abstract;
using MarkdownParser.Infrastructure.Tokenization.Abstract;

namespace MarkdownParser.Concrete.Link
{
    public class MdElementLink : MarkdownElement
    {
        public MdElementLink(Token content) : base(new[] {content})
        {
        }
    }
}