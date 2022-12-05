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
        private readonly List<IToken> tokens;

        public ParsedTextBlock(List<IToken> tokens)
        {
            this.tokens = tokens;
        }

        public string ToHtml()
        {
            var htmlTokens = new List<IToken>();

            if (tokens.FirstOrDefault(el => el is MdHeaderTag) is Tag headerTag)
            {
                htmlTokens = ToHtmlWithReplaceSingleTagToPairedTag(headerTag, tagPosition => new HtmlHeaderTag(tagPosition));
            }
            else
            {
                //htmlTokens.Add(new HtmlParagraphTag(TagPosition.End));
                htmlTokens.AddRange(tokens.Select(token => token.ToHtml()));
                //htmlTokens.Add(new HtmlParagraphTag(TagPosition.End));
            }
            return string.Join(null, htmlTokens.Select(e => e.ToString()));
        }



        private List<IToken> ToHtmlWithReplaceSingleTagToPairedTag(Tag tag, Func<TagPosition, PairedTag> pairedTagCtor)
        {
            var tagPosition = tokens.IndexOf(tag);
            var htmlTokens = new List<IToken>();
            htmlTokens.AddRange(tokens.Take(tagPosition).Select(token => token.ToHtml()));
            htmlTokens.Add(pairedTagCtor(TagPosition.Start));
            htmlTokens.AddRange(tokens.Skip(tagPosition + 1).Select(token => token.ToHtml()));
            htmlTokens.Add(pairedTagCtor(TagPosition.End));
            return htmlTokens;
        }
    }
}
