using Markdown.Dto;

namespace Markdown.TokensUtils
{
    public static class TagRender
    {
        public static string Wrap(TokenType type, string content, string url = null)
            => type switch
            {
                TokenType.Italic => $"<em>{content}</em>",
                TokenType.Strong => $"<strong>{content}</strong>",
                TokenType.Escape => $"<escaped>{content}</escaped>",
                TokenType.Header => $"<h1>{content}</h1>",
                TokenType.Link => WrapLink(content),
                _ => content
            };

        private static string WrapLink(string content)
        {
            var parts = content.Split(["]("], StringSplitOptions.None);
            return parts.Length != 2
                ? throw new ArgumentException("Invalid link format")
                : $"<a href=\"{parts[1]}\">{parts[0]}</a>";
        }
    }
}