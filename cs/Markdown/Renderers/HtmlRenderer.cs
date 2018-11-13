using System;
using System.Collections.Generic;
using System.Text;

namespace Markdown
{
    public class HtmlRenderer : IRenderer
    {
        private readonly Dictionary<MdType, string> mdTypeHtmlSpec = new Dictionary<MdType, string>
        {
            {MdType.Text, ""},
            {MdType.OpenEmphasis, "<ul>"},
            {MdType.CloseEmphasis, "</ul>"},
            {MdType.OpenStrongEmphasis, "<strong>"},
            {MdType.CloseStrongEmphasis, "</strong>"},
        };

        public string Render(Token[] tokens)
        {

            if (tokens == null)
            {
                throw new ArgumentException("Given tokens can't be null", nameof(tokens));
            }

            var result = new StringBuilder();

            foreach (var token in tokens)
            {
                var type = mdTypeHtmlSpec[token.Type];

                result.Append(type);

                if (token.Type == MdType.Text)
                {
                    result.Append(token.Value);
                }
            }

            return result.ToString();
        }
    }
}