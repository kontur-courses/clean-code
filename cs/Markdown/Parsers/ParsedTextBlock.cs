using System.Collections.Generic;
using System.Linq;
using Markdown.Parsers.Tokens;
using Markdown.Parsers.Tokens.Tags.Enum;
using Markdown.Parsers.Tokens.Tags.Html;
using Markdown.Parsers.Tokens.Tags.Markdown;

namespace Markdown.Parsers
{
    public class ParsedTextBlock
    {
        public IEnumerable<IToken> Tokens { get; }

        public ParsedTextBlock(IEnumerable<IToken> tokens)
        {
            Tokens = tokens;
        }

        public string ToHtml()
        {
            var htmlTokens = new List<IToken>();
            if (Tokens.FirstOrDefault() is MdHeaderTag)
            {
                htmlTokens.Add(new HtmlHeaderTag(TagPosition.Start));
                htmlTokens.AddRange(Tokens.Skip(2).Select(token => token.ToHtml()));
                htmlTokens.Add(new HtmlHeaderTag(TagPosition.End));
            }
            else
            {
                htmlTokens.AddRange(Tokens.Select(token => token.ToHtml()));
            }
            return string.Join(null, htmlTokens.Select(e => e.ToString()));
        }

    }
}
