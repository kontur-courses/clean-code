using System;
using System.Linq;
using Markdown.Tokens;

namespace Markdown.TokenIdentifiers
{
    public class HeaderTokenIdentifier: TokenIdentifier<MarkdownToken>
    {
        public HeaderTokenIdentifier(string selector) : base(selector)
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
            var newTag = temporaryToken.Value.Split().First() + " ";
            return new HeaderToken(temporaryToken.Value, newTag, temporaryToken.ParagraphIndex,
                temporaryToken.StartIndex);
        }

        private bool TagPartContainsOnlyTags(string tagPart)
            => tagPart.All(t => t == char.Parse(Selector));

        private bool HasOtherCharsAfterTags(string[] paragraphParts)
            => paragraphParts.Length > 1;
    }
}