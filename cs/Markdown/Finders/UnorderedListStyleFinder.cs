using System;
using System.Collections.Generic;
using System.Linq;

namespace Markdown.Finders
{
    public class UnorderedListStyleFinder : ITokensFinder
    {
        public readonly Style MdStyle;
        public readonly string Text;
        private readonly ITokensFinder ulElementsFinder;

        private int ulListEnd = -1;
        private int ulListStart = -1;

        public UnorderedListStyleFinder(Style mdStyle, TextInfo textInfo, ITokensFinder ulElementsFinder)
        {
            MdStyle = mdStyle;
            Text = textInfo.Text;
            this.ulElementsFinder = ulElementsFinder;
        }

        public IEnumerable<Token> Find()
        {
            var ulElementsTokens = ulElementsFinder.Find().OrderBy(token => token.TokenStart).ToList();
            foreach (var token in ulElementsTokens)
            {
                if (ulListStart == -1)
                    ulListStart = token.TokenStart;
                if (!IsElementOfCurrentUl(token))
                {
                    yield return GetUlToken(ulListStart, ulListEnd);
                    ulListStart = -1;
                    ulListEnd = -1;
                }

                if (!ContainsOnlyOneElement(token))
                    yield return GetUlToken(Text.GetEndOfParagraphPosition(token.ContentStart),
                        token.ContentStart + token.ContentLength);

                ulListEnd = Text.GetEndOfParagraphPosition(token.TokenStart);
            }

            if (ulListStart != -1) yield return GetUlToken(ulListStart, ulListEnd);
        }

        private Token GetUlToken(int listStart, int listEnd)
        {
            return new Token(MdStyle,
                listStart,
                listEnd - listStart,
                listStart,
                listEnd - listStart);
        }

        private bool ContainsOnlyOneElement(Token token)
        {
            return Text.GetEndOfParagraphPosition(token.TokenStart) == token.TokenStart + token.TokenLength;
        }

        private bool IsElementOfCurrentUl(Token token)
        {
            if (ulListEnd == -1)
                return true;
            return token.TokenStart - Environment.NewLine.Length == ulListEnd;
        }
    }
}