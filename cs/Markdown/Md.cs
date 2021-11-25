using Markdown.TagStore;
using Markdown.Tokens;

namespace Markdown
{
    public class Md
    {
        private readonly IConverter converter;
        public Md(IConverter converter)
        {
            this.converter = converter;
        }
        public string Render(string text)
        {
            var converted = converter.Convert(text);
            return converted;
        }
    }
}