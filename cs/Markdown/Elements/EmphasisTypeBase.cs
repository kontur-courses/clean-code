using System;

namespace Markdown.Elements
{
    public abstract class EmphasisTypeBase : IElementType
    {
        public abstract string Indicator { get; }

        public virtual bool IsOpeningOfElement(string markdown, bool[] isEscapedCharAt, int position)
        {
            if (isEscapedCharAt[position])
                return false;

            int positionAfterIndicator = position + Indicator.Length;
            return IsIndicatorAt(markdown, isEscapedCharAt, position) &&
                   markdown.IsNonWhitespaceAt(positionAfterIndicator);
        }

        public virtual bool IsClosingOfElement(string markdown, bool[] isEscapedCharAt, int position)
        {
            if (isEscapedCharAt[position])
                return false;

            return IsIndicatorAt(markdown, isEscapedCharAt, position) &&
                   markdown.IsNonWhitespaceAt(position - 1);
        }

        public abstract bool CanContainElement(IElementType element);
        public abstract bool IsIndicatorAt(string markdown, bool[] isEscapedCharAt, int position);
    }
}
