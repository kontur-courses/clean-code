using Markdown.Infrastructure.Tags;

namespace Markdown.Infrastructure.Parsers
{
    public interface ITagParser
    {
        public ITag Parse(string text);
    }
}