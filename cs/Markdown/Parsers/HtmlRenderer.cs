using Markdown.Models;
using Markdown.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;


namespace Markdown
{

    internal class HtmlRenderer : IHtmlRenderer
    {
        public HtmlRenderer()
        {

        }

        public string Render(List<IToken> tokens)
        {
            if (tokens == null)
                throw new ArgumentNullException();

            if (tokens.Count == 0)
                return string.Empty;

            var result = new StringBuilder();

            foreach (var token in tokens )
            {
                if (token is TextToken textToken)
                    result.Append(RenderTextToken(textToken));

                if (token is TagToken tagToken)
                    result.Append(RenderTagToken(tagToken));
            }

            return result.ToString();
        }
        
        internal string RenderTagToken(TagToken token)
        {
            var inner = Render(token.Children);
            return $"{token.Tag.OpeningTag}{inner}{token.Tag.ClosingTag}";
        }

        internal string RenderTextToken(TextToken token) => token.Content;
    }
}
