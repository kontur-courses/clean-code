using Markdown.Interfaces;

namespace Markdown
{
    public class Renderer : IRenderer
    {
        private readonly ITokenParser tokenParser;
        private readonly IBuilder builder;
        public Renderer(ITokenParser tokenParser, IBuilder builder)
        {
            this.tokenParser = tokenParser;
            this.builder = builder;
        }
        
        public string Render(string data)
        {
            if (string.IsNullOrEmpty(data))
                return data;
            
            var tokens = tokenParser.Parse(data);

            return builder.Build(tokens);
        }
    }
}