using System.Collections.Generic;
using System.Text;
using Markdown.Interfaces;

namespace Markdown.Html
{
    public class HtmlBuilder : IBuilder
    {
        private readonly StringBuilder htmlTextBuilder = new ();

        public string Build(IEnumerable<Token> tokens)
        {
            foreach (var token in tokens)
                htmlTextBuilder.Append(BuildToken(token));

            return htmlTextBuilder.ToString();
        }

        private string BuildToken(Token token)
        {
            var tokenContentBuilder = new StringBuilder();
            
            tokenContentBuilder.Append(token.Tag.Opening);
            tokenContentBuilder.Append(token.Content);

            foreach (var children in token.Childrens)
            {
                tokenContentBuilder.Append(BuildToken(children));
            }

            tokenContentBuilder.Append(token.Tag.Closing);

            return tokenContentBuilder.ToString();
        }
    }
}