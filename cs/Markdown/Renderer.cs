using Markdown.Interfaces;

namespace Markdown
{
    public static class Renderer
    {
        public static string Render<TParser>(string markdownText)
            where TParser : ITokenParser, new()
        {
            if (string.IsNullOrEmpty(markdownText))
                return markdownText;
            
            var tokens = new TParser().Parse(markdownText);
            
            return Builder.Build(tokens);
        }
    }
}