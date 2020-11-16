using System.Collections.Generic;

namespace Markdown
{
    public abstract class MdStyleFinder : ITokensFinder
    {
        public readonly Style MdStyle;
        public readonly string Text;

        protected int Pointer;
        protected TextInfo TextInfo;

        private List<Token> tokensCache;

        protected MdStyleFinder(Style mdStyle, TextInfo textInfo)
        {
            MdStyle = mdStyle;
            Text = textInfo.Text;
            TextInfo = textInfo;
        }

        public virtual IEnumerable<Token> Find()
        {
            if (tokensCache != null)
            {
                foreach (var token in tokensCache)
                    yield return token;
                yield break;
            }

            tokensCache = new List<Token>();
            while (true)
            {
                var tokenPositions = GetNextTagPairPositions();
                if (tokenPositions.Start == -1 || tokenPositions.End == -1)
                    yield break;
                Pointer = tokenPositions.End + MdStyle.EndTag.Length;
                var token = Token.Create(MdStyle, tokenPositions.Start, tokenPositions.End);
                tokensCache.Add(token);
                yield return token;
            }
        }

        protected abstract (int Start, int End) GetNextTagPairPositions();

        protected bool IsEmptyStringInside(int startTagPosition, int endTagPosition)
        {
            return endTagPosition - startTagPosition - MdStyle.StartTag.Length == 0;
        }

        protected bool IsEscaped(int startPosition)
        {
            return TextInfo.EscapedMarkupCharsPositions.Contains(startPosition);
        }
    }
}