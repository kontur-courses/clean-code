using Markdown.Interfaces;

namespace Markdown
{
    public class Renderer
    {
        private readonly ITokenParser tokenParser;
        private readonly IBuilder builder;
        public Renderer(ITokenParser tokenParser, IBuilder builder)
        {
            this.tokenParser = tokenParser;
            this.builder = builder;
        }
        
        public string Render(string markdownText)
        {
            if (string.IsNullOrEmpty(markdownText))
                return markdownText;
            
            var tokens = tokenParser.Parse(markdownText);

            return builder.Build(tokens);
        }
    }
}