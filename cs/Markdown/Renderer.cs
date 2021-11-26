using System;
using System.Linq;
using System.Text;
using Markdown.Interfaces;

namespace Markdown
{
    public class Renderer : ITokenRenderer
    {
        public string Render(TokenTree[] trees)
        {
            var htmlBuilder = new StringBuilder();

            foreach (var component in trees)
            {
                htmlBuilder.Append(ToString(component));
            }

            return htmlBuilder.ToString();
        }

        private string ToString(TokenTree token)
        {
            if (token.Children.Count == 0)
                return token.Value;
            var tag = GetTag(token.TokenType);
            return new StringBuilder()
                .Append(token.TokenType == TokenType.Link 
                    ? GEtImage(token) 
                    : ConvertValueToHtml(tag,string.Join("", token.Children.Select(ToString))))
                .ToString();
        }

        private string GEtImage(TokenTree token) => $"{string.Join("", token.Children.Select(x => ToString(x)))} href=\"{token.Value}\"";
        
        private string GetTag(TokenType type)
        {
            return type switch
            {
                TokenType.Header1 => "h1",
                TokenType.Italics => "em",
                TokenType.Strong => "strong",
                TokenType.Link => "",
                _ => throw new ArgumentException($"Unsupported token: {type}")
            };
        }
        
        private string ConvertValueToHtml(string tag, string value) => $"<{tag}>{value}</{tag}>";
        
    }
}