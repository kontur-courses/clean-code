using System;
using System.Collections.Generic;

namespace Markdown
{
    public class MdUnorderedListElementStyleFinder : ITokensFinder
    {
        public readonly Style MdStyle;
        public readonly string Text;

        private readonly TextInfo textInfo;

        private readonly Stack<(int tokenStart, int contentStart)> ulElements =
            new Stack<(int tokenStart, int tokenElement)>();

        private int pointer;
        private List<Token> tokensCache;
        private int ulElementEnd = -1;

        public MdUnorderedListElementStyleFinder(Style mdStyle, TextInfo textInfo)
        {
            MdStyle = mdStyle;
            this.textInfo = textInfo;
            Text = textInfo.Text;
        }

        public IEnumerable<Token> Find()
        {
            if (tokensCache != null)
                foreach (var token in tokensCache)
                    yield return token;

            tokensCache = new List<Token>();
            while (true)
            {
                var nextUnorderedListElement = GetNextUnorderedListElement();
                if (nextUnorderedListElement.elementStart == -1 || nextUnorderedListElement.contentStart == -1)
                    break;

                if (ulElements.Count != 0)
                {
                    int minWhiteSpaceCount;
                    if (IsElementOfCurrentUl(nextUnorderedListElement.elementStart))
                        minWhiteSpaceCount = nextUnorderedListElement.contentStart -
                                             nextUnorderedListElement.elementStart - 2;
                    else
                        minWhiteSpaceCount = -1;
                    foreach (var token in GetUlElementsTokensFromStack(minWhiteSpaceCount))
                    {
                        tokensCache.Add(token);
                        yield return token;
                    }
                }

                ulElementEnd = Text.GetEndOfParagraphPosition(nextUnorderedListElement.elementStart);
                ulElements.Push(nextUnorderedListElement);
            }

            if (ulElements.Count == 0)
                yield break;
            foreach (var token in GetUlElementsTokensFromStack(-1))
            {
                tokensCache.Add(token);
                yield return token;
            }
        }

        private bool IsElementOfCurrentUl(int elementStart)
        {
            if (ulElementEnd == -1)
                return true;
            return ulElementEnd == elementStart - Environment.NewLine.Length;
        }

        private IEnumerable<Token> GetUlElementsTokensFromStack(int minWhiteSpaceCount)
        {
            while (ulElements.Count > 0)
            {
                var previousUlElement = ulElements.Peek();
                if (previousUlElement.contentStart - previousUlElement.tokenStart - 2 < minWhiteSpaceCount) break;
                ulElements.Pop();
                yield return new Token(
                    MdStyle,
                    previousUlElement.tokenStart,
                    ulElementEnd - previousUlElement.tokenStart,
                    previousUlElement.contentStart,
                    ulElementEnd - previousUlElement.contentStart);
            }
        }

        private (int elementStart, int contentStart) GetNextUnorderedListElement()
        {
            while (true)
            {
                if (pointer == -1)
                    return (-1, -1);
                var startTagPosition = Text.IndexOf(MdStyle.StartTag, pointer);
                if (startTagPosition == -1)
                    return (-1, -1);
                pointer = Text.GetNextParagraphStart(startTagPosition + 1);
                if (startTagPosition < Text.Length - 1 && !char.IsWhiteSpace(Text[startTagPosition + 1]))
                    continue;
                var currentParagraphStart = Text.GetCurrentParagraphStart(startTagPosition);
                if (Text
                    .Substring(currentParagraphStart, startTagPosition - currentParagraphStart)
                    .ContainsOnlyWhiteSpace())
                    return (currentParagraphStart, Math.Min(startTagPosition + 2, Text.Length));
            }
        }
    }
}