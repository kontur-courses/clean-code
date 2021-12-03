using System;
using System.Linq;
using Markdown.Parser;
using Markdown.Tokens;

namespace Markdown.TokenIdentifiers
{
    public class HeaderTokenIdentifier: TokenIdentifier<MarkdownToken>
    {
        public HeaderTokenIdentifier(IParser<MarkdownToken> parser, string selector) : base(parser, selector)
        {
        }

        protected override bool IsValid(string[] paragraphs, TemporaryToken temporaryToken)
        {
            var paragraph = paragraphs[temporaryToken.ParagraphIndex];
            var paragraphParts = paragraph.Split();
            return paragraph.StartsWith(Selector)
                   && TagPartContainsOnlyTags(paragraphParts[0])
                   && HasOtherCharsAfterTags(paragraphParts);
        }

        public override HeaderToken CreateToken(TemporaryToken temporaryToken)
        {
            var newTag = temporaryToken.Value.Split().First();
            var innerValue = temporaryToken.Value[(newTag.Length + 1)..];
            return new HeaderToken(temporaryToken.Value, newTag, temporaryToken.ParagraphIndex,
                temporaryToken.StartIndex) {SubTokens = Parser.Parse(innerValue)};
        }

        private bool TagPartContainsOnlyTags(string tagPart)
            => tagPart.All(t => t == char.Parse(Selector));

        private static bool HasOtherCharsAfterTags(string[] paragraphParts)
            => paragraphParts.Length > 1;
    }
}