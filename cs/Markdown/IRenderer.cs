using Markdown.Infrastructure.Parsers;

namespace Markdown
{
    public interface IRenderer
    {
        public string Render(string markdownText);
    }
}