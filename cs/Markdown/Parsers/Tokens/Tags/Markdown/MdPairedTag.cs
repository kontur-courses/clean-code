using System;
using Markdown.Parsers.Tokens.Tags.Enum;
using Markdown.Extensions;
using System.Collections.Generic;
using System.Linq;

namespace Markdown.Parsers.Tokens.Tags.Markdown
{
    public abstract class MdPairedTag : PairedTag
    {
        public bool IntoWord { get; private set; }
        protected MdPairedTag(MdPairedTag startTag, string data) : 
            base(startTag == null ? TagPosition.Start : TagPosition.End, data)
        {
            if(startTag != null)
            {
                Pair = startTag;
                startTag.Pair = this;
            }
        }

        protected MdPairedTag(TagPosition tagPosition, string data) : base(tagPosition, data)
        {
        }

        public override bool IsValidTag(string currentLine, int currentPosition)
        {
            return position == TagPosition.Start && IsValidTagStartPosition(currentLine, currentPosition)
                   || position == TagPosition.End && IsValidTagEndPosition(currentLine, currentPosition);
        }

        private bool IsValidTagStartPosition(string currentLine, int currentPosition)
        {
            var mdTags = MdTags.GetInstance();
            return !currentLine.IsWhiteSpaceIn(currentPosition)
                && !currentLine.Is(mdTags.IsServiceSymbol, currentPosition);
        }

        private bool IsValidTagEndPosition(string currentLine, int currentPosition)
        {
            var mdTags = MdTags.GetInstance();
            var previousPosition = GetPreviousPosition(currentPosition);
            return  !currentLine.IsWhiteSpaceIn(previousPosition)
                && (!currentLine.Is(mdTags.IsServiceSymbol, previousPosition) || MdCommentTag.IsCommented(currentLine, currentPosition))
                && !currentLine.Is(mdTags.IsServiceSymbol, currentPosition);
        }

        public void CheckInWord(string currentLine, int currentPosition)
        {
            var previousPosition = GetPreviousPosition(currentPosition);
            IntoWord = position == TagPosition.Start 
                       && !currentLine.IsWhiteSpaceIn(previousPosition)
                       && !MdCommentTag.IsCommented(currentLine, currentPosition)
                       && !currentLine.IsWhiteSpaceIn(currentPosition)
                       && currentLine.IsInside(previousPosition);
        }

        public bool IsIntoWord(List<IToken> tokens)
        {
            if (position != TagPosition.End)
                throw new InvalidOperationException();

            var lastToken = tokens.LastOrDefault() as TextToken;
            var startTagPosition = tokens.Count - 2;
            var isStartTagBeforeLastToken = startTagPosition >= 0 && tokens[startTagPosition] == Pair;
            return lastToken?.IsWord() == true && isStartTagBeforeLastToken;
        }

        private int GetPreviousPosition(int currentPosition) => 
            currentPosition - text.Length - 1;
    }
}
