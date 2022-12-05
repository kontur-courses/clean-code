using System;
using Markdown.Parsers.Tokens.Tags.Enum;
using Markdown.Extensions;
using System.Collections.Generic;
using System.Linq;
using Markdown.Parsers.Tokens.Markdown;

namespace Markdown.Parsers.Tokens.Tags.Markdown
{
    public abstract class MdPairedTag : PairedTag
    {
        private readonly bool intoWord;

        protected MdPairedTag(MarkdownParsingLine context, string data) :
            this(context?.OpenedTokens.LastOrDefault(el => el.ToString() == data) as MdPairedTag, data)
        {
            if (context == null) 
                return;

            var previousPosition = GetPreviousPosition(context.CurrentPosition);
            intoWord = Position == TagPosition.Start
                       && !context.Line.IsWhiteSpaceIn(previousPosition)
                       && !MdCommentToken.IsCommented(context.Line, context.CurrentPosition)
                       && !context.Line.IsWhiteSpaceIn(context.CurrentPosition)
                       && context.Line.IsInside(previousPosition);
        }

        private int GetPreviousPosition(int currentPosition) =>
            currentPosition - Text.Length - 1;

        private MdPairedTag(MdPairedTag startTag, string data) : 
            base(startTag == null ? TagPosition.Start : TagPosition.End, data)
        {
            if (startTag == null) 
                return;
            Pair = startTag;
            startTag.Pair = this;
        }

        protected MdPairedTag(TagPosition tagPosition, string data) : base(tagPosition, data)
        {
        }

        public override bool TryToValidate(MarkdownParsingLine context)
        {
            if (!IsValidTag(context.Line, context.CurrentPosition))
                return false;

            if (Pair is null)
                context.OpenedTokens.Add(this);
            else
            {
                if (Pair is MdPairedTag { intoWord: true } && !IsIntoWord(context.Tokens))
                    return false;

                context.OpenedTokens.Remove(Pair);

                if (HasIntersections(context.Tokens))
                    return false;
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
            return !currentLine.IsWhiteSpaceIn(previousPosition) 
                   && !currentLine.Is(mdTags.IsServiceSymbol, previousPosition) 
                   && !currentLine.Is(mdTags.IsServiceSymbol, currentPosition);
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
            for (int endInnerTagIdx = startOuterTagIdx + 1; endInnerTagIdx < tokens.Count; endInnerTagIdx++)
            {
                if (tokens[endInnerTagIdx] is MdPairedTag { Position: TagPosition.End } pairedTag)
                {
                    var startInnerTagIdx = tokens.IndexOf(pairedTag.Pair);
                    if (startOuterTagIdx > startInnerTagIdx)
                    {
                        tokens.ToTextAt(startOuterTagIdx);
                        tokens.ToTextAt(startInnerTagIdx);
                        tokens.ToTextAt(endInnerTagIdx);
                        return true;
                    }
                    else if (pairedTag is MdBoldTag)
                    {
                        tokens.ToTextAt(startInnerTagIdx);
                        tokens.ToTextAt(endInnerTagIdx);
                    }
                }
            }
            return false;
        }
    }
}
