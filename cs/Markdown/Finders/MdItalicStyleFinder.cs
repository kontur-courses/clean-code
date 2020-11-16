using System.Collections.Generic;
using System.Linq;

namespace Markdown
{
    public class MdItalicStyleFinder : MdEmphasisStyleFinder
    {
        private readonly ITokensFinder boldFinder;
        private readonly Style boldStyle;
        private HashSet<int> boldTokensPositions;

        public MdItalicStyleFinder(Style mdStyle, TextInfo textInfo,
            Style boldStyle, ITokensFinder boldFinder) : base(mdStyle, textInfo)
        {
            this.boldStyle = boldStyle;
            this.boldFinder = boldFinder;
        }

        public override IEnumerable<Token> Find()
        {
            boldTokensPositions = boldFinder
                .Find()
                .SelectMany(token => new[] {token.TokenStart, token.ContentStart + token.ContentLength})
                .ToHashSet();
            return base.Find();
        }

        public override bool IsStartTag(int startTagPosition)
        {
            return base.IsStartTag(startTagPosition) && !ContainedInSomeBoldTag(startTagPosition);
        }

        public override bool IsEndTag(int endTagPosition)
        {
            return base.IsEndTag(endTagPosition) && !ContainedInSomeBoldTag(endTagPosition);
        }

        private bool ContainedInSomeBoldTag(int italicTagPosition)
        {
            return IsBoldTag(italicTagPosition - 1)
                   && (IsPartOfBoldStyle(italicTagPosition - 1) || IsUnpairedBoldTag(italicTagPosition - 1))
                   || IsBoldTag(italicTagPosition)
                   && (IsPartOfBoldStyle(italicTagPosition) || IsUnpairedBoldTag(italicTagPosition));
        }

        private bool IsBoldTag(int tagPosition)
        {
            return tagPosition >= 0
                   && Text.IndexOf(boldStyle.StartTag, tagPosition) == tagPosition
                   && !IsEscaped(tagPosition);
        }

        private bool IsPartOfBoldStyle(int tagPosition)
        {
            return boldTokensPositions.Contains(tagPosition);
        }

        private bool IsUnpairedBoldTag(int tagPosition)
        {
            return !IsPartOfBoldStyle(tagPosition)
                   && !IsPartOfBoldStyle(tagPosition - 1);
        }
    }
}