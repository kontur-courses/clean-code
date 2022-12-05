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

        protected MdPairedTag(MarkdownParsingLine context, string data) :
            this(context?.OpenedTokens.LastOrDefault(el => el.ToString() == data) as MdPairedTag, data)
        {
        }

        private MdPairedTag(MdPairedTag startTag, string data) : 
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

        public override bool TryToValidate(MarkdownParsingLine context)
        {
            if (!IsValidTag(context.Line, context.CurrentPosition))
                return false;
            else
            {
                CheckInWord(context.Line, context.CurrentPosition);

                if (Pair is null)
                    context.OpenedTokens.Add(this);
                else
                {
                    if (Pair is MdPairedTag { IntoWord: true } && !IsIntoWord(context.Tokens))
                        return false;

                    context.OpenedTokens.Remove(Pair);

                    if (HasIntersections(context.Tokens))
                        return false;
                }
            }

            return true;
        }

        protected override bool IsValidTag(string currentLine, int currentPosition)
        {
            return Position == TagPosition.Start && IsValidTagStartPosition(currentLine, currentPosition)
                   || Position == TagPosition.End && IsValidTagEndPosition(currentLine, currentPosition);
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

        private void CheckInWord(string currentLine, int currentPosition)
        {
            var previousPosition = GetPreviousPosition(currentPosition);
            IntoWord = Position == TagPosition.Start 
                       && !currentLine.IsWhiteSpaceIn(previousPosition)
                       && !MdCommentTag.IsCommented(currentLine, currentPosition)
                       && !currentLine.IsWhiteSpaceIn(currentPosition)
                       && currentLine.IsInside(previousPosition);
        }

        private bool IsIntoWord(List<IToken> tokens)
        {
            if (Position != TagPosition.End)
                throw new InvalidOperationException();

            var lastToken = tokens.LastOrDefault() as TextToken;
            var startTagPosition = tokens.Count - 2;
            var isStartTagBeforeLastToken = startTagPosition >= 0 && tokens[startTagPosition] == Pair;
            return lastToken?.IsWord() == true && isStartTagBeforeLastToken;
        }

        private bool HasIntersections(List<IToken> tokens)
        {
            if (this.Position != TagPosition.End)
                return false;

            var startOuterTagIdx = tokens.IndexOf(Pair);
            for (int endIdxOfInnerTag = startOuterTagIdx + 1; endIdxOfInnerTag < tokens.Count; endIdxOfInnerTag++)
            {
                if (tokens[endIdxOfInnerTag] is MdPairedTag { Position: TagPosition.End } pairedTag)
                {
                    var startInnerTagIdx = tokens.IndexOf(pairedTag.Pair);
                    if (startOuterTagIdx > startInnerTagIdx)
                    {
                        tokens[startOuterTagIdx] = tokens[startOuterTagIdx].ToText();
                        tokens[startInnerTagIdx] = tokens[startInnerTagIdx].ToText();
                        tokens[endIdxOfInnerTag] = tokens[endIdxOfInnerTag].ToText();
                        return true;

                    }
                    else if (pairedTag is MdBoldTag)
                    {
                        tokens[startInnerTagIdx] = tokens[startInnerTagIdx].ToText();
                        tokens[endIdxOfInnerTag] = tokens[endIdxOfInnerTag].ToText();
                    }

                }
            }

            return false;
        }

        private int GetPreviousPosition(int currentPosition) => 
            currentPosition - Text.Length - 1;
    }
}
