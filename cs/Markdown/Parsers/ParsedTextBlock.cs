using System;
using System.Collections.Generic;
using System.Linq;
using Markdown.Parsers.Tokens;
using Markdown.Parsers.Tokens.Tags;
using Markdown.Parsers.Tokens.Tags.Enum;
using Markdown.Parsers.Tokens.Tags.Html;
using Markdown.Parsers.Tokens.Tags.Markdown;

namespace Markdown.Parsers
{
    public class ParsedTextBlock
    {
        public List<IToken> Tokens { get; }

        public ParsedTextBlock(List<IToken> tokens)
        {
            Tokens = tokens;
        }

        public string ToHtml()
        {
            var htmlTokens = new List<IToken>();

            if (Tokens.FirstOrDefault(el => el is MdHeaderTag) is Tag headerTag)
            {
                htmlTokens = ToHtmlWithReplaceSingleTagToPairedTag(headerTag, tagPosition => new HtmlHeaderTag(tagPosition));
            }
            else
            {
                htmlTokens.AddRange(Tokens.Select(token => token.ToHtml()));
            }
            return string.Join(null, htmlTokens.Select(e => e.ToString()));
        }



        private List<IToken> ToHtmlWithReplaceSingleTagToPairedTag(Tag tag, Func<TagPosition, PairedTag> pairedTagCtor)
        {
            var tagPosition = Tokens.IndexOf(tag);
            var htmlTokens = new List<IToken>();
            htmlTokens.AddRange(Tokens.Take(tagPosition).Select(token => token.ToHtml()));
            htmlTokens.Add(pairedTagCtor(TagPosition.Start));
            htmlTokens.AddRange(Tokens.Skip(tagPosition + 1).Select(token => token.ToHtml()));
            htmlTokens.Add(pairedTagCtor(TagPosition.End));
            return htmlTokens;
        }
    }
}
