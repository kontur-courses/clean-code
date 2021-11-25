using System.Collections.Generic;
using System.Text;
using Markdown.Tokens;

namespace Markdown
{
    public class TokenToHtmlTranslator : ITokenTranslator
    {
        public string Translate(IEnumerable<IToken> tokens)
        {
            var htmlMarkup = new StringBuilder();

            foreach (var token in tokens)
            {
                if (token.ShouldShowValue)
                    htmlMarkup.Append(token.Value);
                else
                    htmlMarkup.Append(token.IsOpening
                        ? token.OpeningTag
                        : token.ClosingTag);
            }
            return htmlMarkup.ToString();
        }
    }
}
