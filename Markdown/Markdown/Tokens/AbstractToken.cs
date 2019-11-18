using System;

namespace Markdown.Tokens
{
    public class AbstractToken : IToken, IComparable
    {
        protected AbstractToken(string text, int indexTokenStart, IToken[] nestedTokens, int length)
        {
            Text = text;
            IndexTokenStart = indexTokenStart;
            NestedTokens = nestedTokens;
            Length = length;
        }
        public int IndexTokenStart { get; }
        public string Text { get; }
        public IToken[] NestedTokens { get; }
        public int Length { get; }

        public override bool Equals(object obj)
        {
            if (obj.GetType() != this.GetType())
                return false;
            var abstractToken = (AbstractToken) obj;
            for (var i = 0; i < NestedTokens.Length; i++)
                if (!NestedTokens[i].Equals(abstractToken.NestedTokens[i]))
                {
                    return false;
                }

            return IndexTokenStart == abstractToken.IndexTokenStart && Text == abstractToken.Text;
        }

        public override int GetHashCode()
        {
            return IndexTokenStart.GetHashCode() & Text.GetHashCode() & NestedTokens.GetHashCode();
        }

        public int CompareTo(object obj)
        {
            var abstractToken = (AbstractToken) obj;
            return abstractToken.Text.Length.CompareTo(Text.Length) * - 1;
        }
    }
}